using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room that is in xy offset coordinates.
/// </summary>
public class RectangularRoom : Room
{
    public int X1 { get; private set; }
    public int Y1 { get; private set; }
    public int X2 { get; private set; }
    public int Y2 { get; private set; }

    /// <summary>
    /// Creates a room based on minX, minY positions
    /// </summary>
    /// <param name="minX"></param>
    /// <param name="minY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public RectangularRoom(
        int minX, int minY, int width, int height)
    {
        X1 = minX;
        Y1 = minY;
        X2 = minX + width;
        Y2 = minY + height;

        CreateInnerArea();
        SetCenter();
    }

    private void SetCenter()
    {
        int centerX = (X1 + X2) / 2;
        int centerY = (Y1 + Y2) / 2;
        Center = (centerX, centerY);
    }

    protected override void CreateInnerArea()
    {
        InnerArea = new List<(int, int)>();

        for (int x = X1 + 1; x < X2; x++)
        {
            for (int y = Y1 + 1; y < Y2; y++)
            {
                InnerArea.Add((x, y));
            }
        }
    }

    public bool IntersectsRectangularRoom(RectangularRoom otherRoom)
    {
        return X1 <= otherRoom.X2 &&
               X2 >= otherRoom.X1 &&
               Y1 <= otherRoom.Y2 &&
               Y2 >= otherRoom.Y1;
    }
}
