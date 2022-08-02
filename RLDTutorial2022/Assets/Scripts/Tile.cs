using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Floor, Wall, DownStairs };
[Serializable]
public class Tile
{
    private Action<Tile> cbOnVisibilityChanged;
    private Action<Tile> cbOnHighlighted;
    private Action<Tile> cbOnDehighlighted;

    public Color backgroundColor;
    public Color foregroundColor;

    public HexCoordinates Coordinates { get; private set; }

    public Entity entity;
    public Item item;
    public char Character { get; private set; }

    public bool IsWalkable { get; private set; }
    public bool IsTransparent { get; private set; }

    public VisibilityLevel VisibilityLevel { get; private set; }
    public void ChangeVisibilityLevel(VisibilityLevel vl)
    {
        VisibilityLevel = vl;
        cbOnVisibilityChanged?.Invoke(this);
    }

    public Tile(TileType type, HexCoordinates coordinates)
    {
        Coordinates = coordinates;
        entity = null;
        item = null;
        VisibilityLevel = VisibilityLevel.NotVisible;

        switch (type)
        {
            case TileType.Floor:
                CreateFloorTile();
                break;
            
            case TileType.Wall:
                CreateWallTile();
                break;

            case TileType.DownStairs:
                CreateDownStairsTile();
                break;

            default:
                Debug.LogError("No tile type");
                break;
        }
    }

    /// <summary>
    /// Constructor to create tile from loaded data.
    /// </summary>
    /// <param name="savedTile"></param>
    public Tile (SavedTile savedTile)
    {
        backgroundColor = SavedColor.LoadColor(
            savedTile.backgroundColor);
        foregroundColor = SavedColor.LoadColor(
            savedTile.foregroundColor);
        Coordinates = savedTile.coordinates;

        Character = savedTile.character;
        IsWalkable = savedTile.isWalkable;
        IsTransparent = savedTile.isTransparent;
        VisibilityLevel = (VisibilityLevel)savedTile.visibilityLevel;
    }

    private void CreateFloorTile()
    {
        backgroundColor = ColorDatabase.defaultTileBackground;
        foregroundColor = ColorDatabase.defaultTileForeground;
        IsWalkable = true;
        IsTransparent = true;
        Character = ' ';
    }

    private void CreateWallTile()
    {
        backgroundColor = ColorDatabase.defaultTileWallBackground;
        foregroundColor = ColorDatabase.defaultTileForeground;
        IsWalkable = false;
        IsTransparent = false;
        Character = '#';
    }

    private void CreateDownStairsTile()
    {
        backgroundColor = ColorDatabase.defaultTileBackground;
        foregroundColor = ColorDatabase.defaultTileForeground;
        IsWalkable = true;
        IsTransparent = true;
        Character = '>';
    }

    public Tile[] GetNeighboringTiles()
    {
        Tile[] neighbours = new Tile[6];
        for (int i = 0; i < neighbours.Length; i++)
        {
            // Cast i as each direction
            neighbours[i] = GameManager.Instance.CurrentGrid
                .GetTileInDirection(
                this, (Direction)i);
        }

        return neighbours;
    }

    public void Highlight()
    {
        cbOnHighlighted?.Invoke(this);
    }

    public void Dehighlight()
    {
        cbOnDehighlighted?.Invoke(this);
    }

    public void RegisterOnVisibilityChanged(
        Action<Tile> callbackfunc)
    {
        cbOnVisibilityChanged += callbackfunc;
    }

    public void UnregisterOnVisibilityChanged(
        Action<Tile> callbackfunc)
    {
        cbOnVisibilityChanged -= callbackfunc;
    }

    public void RegisterOnHighlighted(
    Action<Tile> callbackfunc)
    {
        cbOnHighlighted += callbackfunc;
    }

    public void UnregisterOnHighlighted(
        Action<Tile> callbackfunc)
    {
        cbOnHighlighted -= callbackfunc;
    }

    public void RegisterOnDehighlighted(
        Action<Tile> callbackfunc)
    {
        cbOnDehighlighted += callbackfunc;
    }

    public void UnregisterOnDehighlighted(
        Action<Tile> callbackfunc)
    {
        cbOnDehighlighted -= callbackfunc;
    }
}
