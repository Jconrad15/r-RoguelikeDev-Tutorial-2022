using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static readonly Color defaultColor = Color.white;

    public HexCoordinates coordinates;
    public Color color;

    public Tile(HexCoordinates coordinates)
    {
        this.coordinates = coordinates;

        if (coordinates.Z % 2 == 0)
        {
            color = defaultColor;
        }
        else
        {
            color = Color.black;
        }
    }
}
