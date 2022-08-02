using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    private Action<Item> cbOnItemCreated;

    public void CreateItems(TileGrid grid, int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        Tile[] roomCenters = grid.GetAllRoomCenterTiles();
        Room[] rooms = grid.GetAllRoomsArray();

        for (int i = 0; i < roomCenters.Length; i++)
        {
            CreateItemsInRoom(grid, rooms[i], seed);
        }

        // Restore state
        Random.state = oldState;
    }

/*    /// <summary>
    /// Create items when transfering player inventory to new level.
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="seed"></param>
    /// <param name="playerInventoryItems"></param>
    public void CreateItems(
        TileGrid grid, int seed, List<Item> playerInventoryItems)
    {
        // Load the player inventory items from previous level
        TransferInventoryItemsToLevel(playerInventoryItems);

        // Normally create items
        CreateItems(grid, seed);
    }*/
    
    public void LoadItems(TileGrid grid, SaveObject saveObject)
    {
        SavedTile[] savedTiles = saveObject.savedTileGrid.savedTiles;
        for (int i = 0; i < savedTiles.Length; i++)
        {
            CheckLoadInventoryItems(grid, savedTiles, i);

            // Check if the tile has an item
            if (savedTiles[i].savedItem == null) { continue; }

            Item loadedItem = Item.SpawnCloneAtTile(
                savedTiles[i].savedItem, grid.Tiles[i]);

            cbOnItemCreated?.Invoke(loadedItem);
            items.Add(loadedItem);
        }
    }

/*    public void TransferInventoryItemsToLevel(
        List<Item> inventoryItems)
    {
        Entity player = GameManager.Instance
            .EntityManager.GetPlayerEntity();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            // Create item at the tile
            Item loadedInventoryItem = Item.SpawnCloneAtTile(
                inventoryItems[i], player.CurrentTile);
            // Entity picks up item to inventory
            player.TryPickUpItem();

            cbOnItemCreated?.Invoke(loadedInventoryItem);
            items.Add(loadedInventoryItem);
        }
    }*/

    private void CheckLoadInventoryItems(
        TileGrid grid, SavedTile[] savedTiles, int index)
    {
        // Check if inventory items were saved for entity on tile
        if (savedTiles[index].savedEntity == null) { return; }

        SavedItem[] savedItems = savedTiles[index]
            .savedEntity.savedItemsFromInventoryComponent;
        if (savedItems == null) { return; }

        // Get the loaded entity and inventory
        Entity loadedEntity = grid.Tiles[index].entity;
        Inventory inv = loadedEntity.TryGetInventoryComponent();

        if (inv == null)
        {
            Debug.LogError(
                "Loading items into null inventory component");
            return;
        }

        for (int j = 0; j < savedItems.Length; j++)
        {
            // Create item at the tile
            Item loadedInventoryItem = Item.SpawnCloneAtTile(
                savedTiles[index].savedItem, grid.Tiles[index]);
            // Entity picks up item to inventory
            loadedEntity.TryPickUpItem();

            cbOnItemCreated?.Invoke(loadedInventoryItem);
            items.Add(loadedInventoryItem);
        }
        
    }

    private void CreateItemsInRoom(
        TileGrid grid, Room room, int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        int itemCount = Random.Range(1, 3);
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

            PlaceItemAtTile(tile, seed);
        }

        // Restore state
        Random.state = oldState;
    }

    private void PlaceItemAtTile(Tile tile, int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        // Use seed for this tile
        seed += tile.Coordinates.X + tile.Coordinates.Z;
        Random.InitState(seed);

        // Randomly choose which item
        Item newItem;
        float randomValue = Random.value;
        if (randomValue < 0.7f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.HealthPotionPrefab, tile);
        }
        else if (randomValue < 0.8f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.FireballScrollPrefab, tile);
        }
        else if (randomValue < 0.9f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.ConfusionScrollPrefab, tile);
        }
        else
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.LightingScrollPrefab, tile);
        }

        cbOnItemCreated?.Invoke(newItem);
        items.Add(newItem);

        // Restore state
        Random.state = oldState;
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
