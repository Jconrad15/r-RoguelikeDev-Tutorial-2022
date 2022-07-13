using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<Entity> entities = new List<Entity>();

    private Action<Entity> cbOnPlayerCreated;

    public void CreateEntities(TileGrid grid)
    {
        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();


        CreatePlayer(roomCenters[0]);

        // Create NPCs -- Use index 1 to N
        for (int i = 1; i < roomCenters.Length; i++)
        {
            CreateEntitiesInRoom(grid, rooms[i]);
        }
    }

    private void CreatePlayer(Tile tile)
    {
        Entity newPlayer = Entity.SpawnCloneAtTile(
            EntityFactory.Instance.PlayerPrefab, tile);

        Debug.Log("player at " +
            tile.Coordinates.ToString());

        entities.Add(newPlayer);
        cbOnPlayerCreated?.Invoke(newPlayer);
    }

    private void CreateEntitiesInRoom(TileGrid grid, Room room)
    {
        int entityCount = 2;
        List<(int, int)> locations = room.InnerArea;

        for (int i = 0; i < entityCount; i++)
        {
            // Determine index and remove from list
            int index = UnityEngine.Random.Range(
                0, locations.Count);
            (int, int) location = locations[index];
            locations.RemoveAt(index);

            Tile tile = grid.GetTileAtPos(
                location.Item1, location.Item2);

            // Skip if this tile already has an entity
            if (tile.entity != null) { continue; }

            PlaceEntityAtTile(tile);
        }

    }

    private void PlaceEntityAtTile(Tile tile)
    {
        Entity newEntity;
        if (UnityEngine.Random.value < 0.8f)
        {
            newEntity = Entity.SpawnCloneAtTile(
                EntityFactory.Instance.OrcPrefab, tile);
        }
        else
        {
            newEntity = Entity.SpawnCloneAtTile(
                EntityFactory.Instance.TrollPrefab, tile);
        }

        entities.Add(newEntity);
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
}
