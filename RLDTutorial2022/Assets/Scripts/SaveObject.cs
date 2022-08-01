using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveObject
{
    public SavedTileGrid savedTileGrid;

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
    public SavedTileGrid(TileGrid tileGrid)
    {
        width = tileGrid.width;
        height = tileGrid.height;

        savedTiles = new SavedTile[tileGrid.Tiles.Length];
        for (int i = 0; i < tileGrid.Tiles.Length; i++)
        {
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

    public SavedTile(Tile tile)
    {
        backgroundColor = new SavedColor(tile.backgroundColor);
        foregroundColor = new SavedColor(tile.foregroundColor);
        coordinates = tile.Coordinates;

        if (savedEntity != null)
        {
            savedEntity = new SavedEntity(tile.entity);
        }
        if (savedItem != null)
        {
            savedItem = new SavedItem(tile.item);
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
    public List<BaseComponent> components;

    public SavedEntity(Entity entity)
    {
        isPlayer = entity.IsPlayer;
        character = entity.Character;
        visibilityDistance = entity.VisibilityDistance;
        entityName = entity.EntityName;
        color = new SavedColor(entity.Color);
        blocksMovement = entity.BlocksMovement;
        components = entity.Components;
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

    public SavedColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }
}
