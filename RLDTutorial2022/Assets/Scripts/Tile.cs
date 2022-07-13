using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Floor, Wall };
public class Tile
{
    private Action<Tile> cbOnVisibilityChanged;

    public Color backgroundColor;
    public static readonly Color defaultBackgroundColor =
        new Color32(15, 76, 92, 255);

    public Color foregroundColor;
    public static readonly Color defaultForegroundColor =
        new Color32(251, 139, 36, 255);

    public static readonly Color defaultWallBackgroundColor =
        new Color32(95, 15, 64, 255);


    public HexCoordinates Coordinates { get; private set; }

    public Entity entity;
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
        VisibilityLevel = VisibilityLevel.NotVisible;

        switch (type)
        {
            case TileType.Floor:
                CreateFloorTile();
                break;
            
            case TileType.Wall:
                CreateWallTile();
                break;
        }
    }

    private void CreateFloorTile()
    {
        backgroundColor = defaultBackgroundColor;
        foregroundColor = defaultForegroundColor;
        IsWalkable = true;
        IsTransparent = true;
        Character = ' ';
    }

    private void CreateWallTile()
    {
        backgroundColor = defaultWallBackgroundColor;
        foregroundColor = defaultForegroundColor;
        IsWalkable = false;
        IsTransparent = false;
        Character = '#';
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
}
