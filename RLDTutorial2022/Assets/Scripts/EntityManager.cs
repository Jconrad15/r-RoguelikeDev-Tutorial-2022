using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<Entity> entities = new List<Entity>();

    private Action<Entity> cbOnPlayerCreated;

    public void CreateEntities(TileGrid grid, Color playerColor)
    {
        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();


        CreatePlayer(playerColor, roomCenters[0]);

        // Create NPCs -- Use index 1 to N
        for (int i = 1; i < roomCenters.Length; i++)
        {
            CreateEntitiesInRoom(grid, rooms[i]);
        }
    }

    private void CreatePlayer(Color playerColor, Tile tile)
    {
        Entity newPlayer = new Entity(
            tile, "@", playerColor, true);

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

            PlaceEntityAtTile(tile);
        }

    }

    private void PlaceEntityAtTile(Tile tile)
    {
        Entity newEntity = new Entity(
            tile, "g", Color.magenta);

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
