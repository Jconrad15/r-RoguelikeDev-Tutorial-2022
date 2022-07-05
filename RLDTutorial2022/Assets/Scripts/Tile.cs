using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Floor, Wall };
public class Tile
{
    public Color backgroundColor;
    public static readonly Color defaultBackgroundColor =
        new Color32(50, 50, 150, 255);

    public Color foregroundColor;
    public static readonly Color defaultForegroundColor =
        Color.white;

    public HexCoordinates Coordinates { get; private set; }

    public Entity entity;
    public char Character { get; private set; }

    public bool IsWalkable { get; private set; }
    public bool IsTransparent { get; private set; }

    public Tile(
        HexCoordinates coordinates,
        bool isWalkable = false,
        bool isTransparent = false,
        char character = ' ')
    {
        Coordinates = coordinates;
        entity = null;

        Character = character;
        backgroundColor = defaultBackgroundColor;
        foregroundColor = defaultForegroundColor;
        IsWalkable = isWalkable;
        IsTransparent = isTransparent;
    }

    public Tile(TileType type, HexCoordinates coordinates)
    {
        Coordinates = coordinates;
        entity = null;

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
        backgroundColor = new Color32(0, 0, 100, 255);
        foregroundColor = defaultForegroundColor;
        IsWalkable = false;
        IsTransparent = false;
        Character = '#';
    }

}
