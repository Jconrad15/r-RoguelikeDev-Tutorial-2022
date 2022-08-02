using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class SaveObject
{
    public SavedTileGrid savedTileGrid;

    [JsonConstructor]
    private SaveObject() { }

    public SaveObject(TileGrid tileGrid)
    {
        savedTileGrid = new SavedTileGrid(tileGrid);
    }
}

[Serializable]
public class SavedTileGrid
{
    public int width;
    public int height;
    public SavedTile[] savedTiles;

    [JsonConstructor]
    private SavedTileGrid() { }

    public SavedTileGrid(TileGrid tileGrid)
    {
        width = tileGrid.width;
        height = tileGrid.height;

        savedTiles = new SavedTile[tileGrid.Tiles.Length];
        for (int i = 0; i < tileGrid.Tiles.Length; i++)
        {
            if (tileGrid.Tiles[i] == null)
            {
                Debug.LogError("Null tile");
                continue;
            }

            savedTiles[i] = new SavedTile(tileGrid.Tiles[i]);
        }

    }
}

[Serializable]
public class SavedTile
{
    public SavedColor backgroundColor;
    public SavedColor foregroundColor;
    public HexCoordinates coordinates;
    public SavedEntity savedEntity;
    public SavedItem savedItem;
    public char character;
    public bool isWalkable;
    public bool isTransparent;
    public int visibilityLevel;

    [JsonConstructor]
    private SavedTile() { }

    public SavedTile(Tile tile)
    {
        backgroundColor = new SavedColor(tile.backgroundColor);
        foregroundColor = new SavedColor(tile.foregroundColor);
        coordinates = tile.Coordinates;

        if (tile.entity != null)
        {
            savedEntity = new SavedEntity(tile.entity);
        }
        else
        {
            savedEntity = null;
        }
        if (tile.item != null)
        {
            savedItem = new SavedItem(tile.item);
        }
        else
        {
            savedItem = null;
        }

        character = tile.Character;
        isWalkable = tile.IsWalkable;
        isTransparent = tile.IsTransparent;
        visibilityLevel = (int)tile.VisibilityLevel;
    }
}

[Serializable]
/// <summary>
/// Saves entities that are in a tile.
/// </summary>
public class SavedEntity
{
    public bool isPlayer;
    public string character;
    public int visibilityDistance;
    public string entityName;
    public SavedColor color;
    public bool blocksMovement;
    public BaseComponent[] components;

    public SavedItem[] savedItemsFromInventoryComponent;

    [JsonConstructor]
    private SavedEntity() { }

    public SavedEntity(Entity entity)
    {
        isPlayer = entity.IsPlayer;
        character = entity.Character;
        visibilityDistance = entity.VisibilityDistance;
        entityName = entity.EntityName;
        color = new SavedColor(entity.Color);
        blocksMovement = entity.BlocksMovement;

        List<BaseComponent> tempComponents = new List<BaseComponent>();
        for (int i = 0; i < entity.Components.Count; i++)
        {
            // Save inventory item components into the saved entity 
            if (entity.Components[i] is Inventory)
            {
                Inventory inventory = (Inventory)entity.Components[i];
                List<Item> items = inventory.GetItems();
                savedItemsFromInventoryComponent =
                    new SavedItem[items.Count];
                for (int j = 0; j < items.Count; j++)
                {
                    savedItemsFromInventoryComponent[j] =
                        new SavedItem(items[j]);
                }
            }

            tempComponents.Add(entity.Components[i]);
        }
        components = tempComponents.ToArray();
    }
}

[Serializable]
/// <summary>
/// Saves items that are in a tile.
/// </summary>
public class SavedItem
{
    public string character;
    public string itemName;
    public SavedColor color;
    public bool blocksMovement;
    public List<BaseItemComponent> components;

    [JsonConstructor]
    private SavedItem() { }

    public SavedItem(Item item)
    {
        character = item.Character;
        itemName = item.ItemName;
        color = new SavedColor(item.Color);
        blocksMovement = item.BlocksMovement;
        components = item.Components;
    }
}

[Serializable]
public class SavedColor
{
    public float r;
    public float g;
    public float b;
    public float a;

    [JsonConstructor]
    private SavedColor() { }

    public SavedColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public static Color LoadColor(SavedColor savedColor)
    {
        return new Color(
            savedColor.r,
            savedColor.g,
            savedColor.b,
            savedColor.a);
    }
}