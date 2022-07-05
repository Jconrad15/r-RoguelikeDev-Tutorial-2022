using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room that is in xy offset coordinates.
/// </summary>
public class RectangularRoom
{
    public int X1 { get; private set; }
    public int Y1 { get; private set; }
    public int X2 { get; private set; }
    public int Y2 { get; private set; }

    public (int, int) Center { get; private set; }

    public (List<int>, List<int>) InnerArea { get; private set; }

    /// <summary>
    /// Creates a room based on minX, minY positions
    /// </summary>
    /// <param name="minX"></param>
    /// <param name="minY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public RectangularRoom(int minX, int minY, int width, int height)
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

    private void CreateInnerArea()
    {
        List<int> x = new List<int>();
        for (int i = X1 + 1; i < X2; i++)
        {
            x.Add(i);
        }

        List<int> y = new List<int>();
        for (int i = Y1 + 1; i < Y2; i++)
        {
            y.Add(i);
        }

        InnerArea = (x, y);
    }

    public bool Contains(int x, int y)
    {
        if (InnerArea.Item1.Contains(x) &&
            InnerArea.Item2.Contains(y))
        {
            return true;
        }

        return false;
    }

    public bool Intersects(RectangularRoom otherRoom)
    {
        return X1 <= otherRoom.X2 &&
               X2 >= otherRoom.X1 &&
               Y1 <= otherRoom.Y2 &&
               Y2 >= otherRoom.Y1;
    }
}
