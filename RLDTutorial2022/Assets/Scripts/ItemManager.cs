using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    private Action<Item> cbOnItemCreated;

    public void CreateItems(TileGrid grid)
    {
        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();

        for (int i = 0; i < roomCenters.Length; i++)
        {
            CreateItemsInRoom(grid, rooms[i]);
        }

        //Debug.Log("Created " + items.Count + " items");
    }

    private void CreateItemsInRoom(TileGrid grid, Room room)
    {
        int itemCount = UnityEngine.Random.Range(1, 3);
        List<(int, int)> locations = room.InnerArea;

        for (int i = 0; i < itemCount; i++)
        {
            // Determine index and remove from list
            int index = UnityEngine.Random.Range(
                0, locations.Count);
            (int, int) location = locations[index];
            locations.RemoveAt(index);

            Tile tile = grid.GetTileAtPos(
                location.Item1, location.Item2);

            // Skip if this tile already has an item
            if (tile.item != null) { continue; }

            PlaceItemAtTile(tile);
        }

    }

    private void PlaceItemAtTile(Tile tile)
    {
        Item newItem;
        if (UnityEngine.Random.value < 0.7)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.HealthPotionPrefab, tile);
        }
        else
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.LightingScrollPrefab, tile);
        }
        
        cbOnItemCreated?.Invoke(newItem);
        items.Add(newItem);
    }

    public List<Item> GetItems()
    {
        // copy items list
        return new List<Item>(items);
    }

    public void RegisterOnItemCreated(
        Action<Item> callbackfunc)
    {
        cbOnItemCreated += callbackfunc;
    }

    public void UnregisterOnItemCreated(
        Action<Item> callbackfunc)
    {
        cbOnItemCreated -= callbackfunc;
    }
}
