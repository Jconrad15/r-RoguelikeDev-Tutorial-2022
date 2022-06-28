using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static readonly Color defaultColor = Color.white;

    public HexCoordinates coordinates;
    public Color color;

    public Entity entity;

    public Tile(HexCoordinates coordinates)
    {
        this.coordinates = coordinates;
        entity = null;

        if (coordinates.X % 2 == 0)
        {
            color = defaultColor;
        }
        else
        {
            color = Color.blue;
        }
    }
}
