using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityManager : MonoBehaviour
{
    private List<Entity> entities = new List<Entity>();

    private Action<Entity> cbOnPlayerCreated;
    private Action<Entity> cbOnEntityCreated;

    public void CreateEntities(
        TileGrid grid, int seed, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();

        CreatePlayer(roomCenters[0]);

        // Create NPCs -- Use index 1 to N
        for (int i = 1; i < roomCenters.Length; i++)
        {
            CreateEntitiesInRoom(grid, rooms[i], seed, floorLevel);
        }

        // Restore state
        Random.state = oldState;
    }

    /// <summary>
    /// Create entities and load player entity from previous level
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="seed"></param>
    /// <param name="playerEntity"></param>
    public void CreateEntities(
        TileGrid grid, int seed,
        Entity playerEntity, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();

        CreatePlayer(roomCenters[0], playerEntity);

        // Create NPCs -- Use index 1 to N
        for (int i = 1; i < roomCenters.Length; i++)
        {
            CreateEntitiesInRoom(grid, rooms[i], seed, floorLevel);
        }

        // Restore state
        Random.state = oldState;
    }

    public void LoadEntities(TileGrid grid, SaveObject saveObject)
    {
        // Load tiles
        SavedTile[] savedTiles = saveObject.savedTileGrid.savedTiles;
        for (int i = 0; i < savedTiles.Length; i++)
        {
            SavedEntity savedEntity = savedTiles[i].savedEntity;
            if (savedEntity == null) { continue; }

            // Add the saved entity to the tile
            Entity loadedEntity = Entity.SpawnCloneAtTile(
                savedEntity, grid.Tiles[i]);

            if (loadedEntity.IsPlayer)
            {
                cbOnPlayerCreated?.Invoke(loadedEntity);
            }
            else
            {
                cbOnEntityCreated?.Invoke(loadedEntity);
            }
            entities.Add(loadedEntity);
        }
    }

    private void CreatePlayer(Tile tile)
    {
        Entity newPlayer = Entity.SpawnCloneAtTile(
            EntityFactory.Instance.PlayerPrefab, tile);

        Debug.Log("Player at " + tile.Coordinates.ToString());

        entities.Add(newPlayer);
        cbOnPlayerCreated?.Invoke(newPlayer);
    }

    /// <summary>
    /// Load player Entity from previous level.
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="playerEntity"></param>
    private void CreatePlayer(Tile tile, Entity playerEntity)
    {
        Entity newPlayer = playerEntity;
        newPlayer.SetTile(tile);
        tile.entity = newPlayer;

        Debug.Log("Player at " + tile.Coordinates.ToString());

        entities.Add(newPlayer);
        cbOnPlayerCreated?.Invoke(newPlayer);
    }

    private void CreateEntitiesInRoom(
        TileGrid grid, Room room, int seed, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        int entityCount = 2;
        List<(int, int)> locations = room.InnerArea;

        for (int i = 0; i < entityCount; i++)
        {
            // Determine index and remove from list
            int index = Random.Range(
                0, locations.Count);
            (int, int) location = locations[index];
            locations.RemoveAt(index);

            Tile tile = grid.GetTileAtPos(
                location.Item1, location.Item2);

            // Skip if this tile already has an entity
            if (tile.entity != null) { continue; }

            PlaceEntityAtTile(tile, seed, floorLevel);
        }

        // Restore state
        Random.state = oldState;
    }

    private void PlaceEntityAtTile(
        Tile tile, int seed, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        // Use seed for this tile
        seed += tile.Coordinates.X + tile.Coordinates.Z;
        Random.InitState(seed);

        // Select entity
        Entity newEntity = EntitySelector.SelectEntityPerLevel(
            tile, seed, floorLevel);

        cbOnEntityCreated?.Invoke(newEntity);
        entities.Add(newEntity);

        // Restore state
        Random.state = oldState;
    }

    public List<Entity> GetNonPlayerEntities()
    {
        List<Entity> nonPlayerEntities = new List<Entity>(entities);
        for (int i = 0; i < nonPlayerEntities.Count; i++)
        {
            if (nonPlayerEntities[i].IsPlayer)
            {
                nonPlayerEntities.Remove(nonPlayerEntities[i]);
                break;
            }
        }

        return nonPlayerEntities;
    }

    public List<Entity> GetAllEntities()
    {
        List<Entity> allEntities = new List<Entity>(entities);
        return allEntities;
    }

    public Entity GetPlayerEntity()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].IsPlayer)
            {
                return entities[i];
            }
        }

        return null;
    }

    public Entity GetNearestFighterEntity(Entity startingEntity)
    {
        Entity[] otherEntities =
            GetOtherEntitiesWithFighterComponent(startingEntity);

        float[] distances = new float[otherEntities.Length];
        for (int i = 0; i < otherEntities.Length; i++)
        {
            float distance = HexCoordinates.HexDistance(
                startingEntity.CurrentTile.Coordinates,
                otherEntities[i].CurrentTile.Coordinates);
            distances[i] = distance;
        }

        int index = Array.IndexOf(distances, distances.Min());

        return otherEntities[index];
    }

    /// <summary>
    /// Gets entities with fighter component
    /// other than the provided starting entity.
    /// </summary>
    /// <param name="startingEntity"></param>
    /// <returns></returns>
    private Entity[] GetOtherEntitiesWithFighterComponent(
        Entity startingEntity)
    {
        List<Entity> tempOtherEntities = new List<Entity>(entities);
        // Remove the starting entity
        tempOtherEntities.Remove(startingEntity);
        // Get entities with fighter components
        for (int i = tempOtherEntities.Count - 1; i >= 0; i--)
        {
            Fighter f = tempOtherEntities[i].TryGetFighterComponent();
            if (f == null)
            {
                tempOtherEntities.Remove(tempOtherEntities[i]);
            }
        }
        return tempOtherEntities.ToArray();
    }

    public void RegisterOnPlayerCreated(
        Action<Entity> callbackfunc)
    {
        cbOnPlayerCreated += callbackfunc;
    }

    public void UnregisterOnPlayerCreated(
        Action<Entity> callbackfunc)
    {
        cbOnPlayerCreated -= callbackfunc;
    }

    public void RegisterOnEntityCreated(
        Action<Entity> callbackfunc)
    {
        cbOnEntityCreated += callbackfunc;
    }

    public void UnregisterOnEntityCreated(
        Action<Entity> callbackfunc)
    {
        cbOnEntityCreated -= callbackfunc;
    }
}
