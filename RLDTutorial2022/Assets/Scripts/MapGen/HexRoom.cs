using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRoom : Room
{
    public int Radius { get; private set; }

    /// <summary>
    /// Center in offset coordinates.
    /// </summary>
    public (int, int) Center { get; private set; }

    public HexRoom(int radius, (int, int) center)
    {
        Radius = radius;
        Center = center;

        CreateInnerArea();
    }

    protected override void CreateInnerArea()
    {
        InnerArea = new List<(int, int)>();

        HexCoordinates centerHexCoords = HexCoordinates
            .FromOffsetCoordinates(Center.Item1, Center.Item2);

        for (int q = -Radius; q <= Radius; q++)
        {
            for (int r = -Radius; r <= Radius; r++)
            {
                for (int s = -Radius; s <= Radius; s++)
                {
                    if (q + r + s == 0)
                    {
                        // Add this hex spot to the inner area
                        HexCoordinates hex = new HexCoordinates(
                            q + centerHexCoords.X, r +centerHexCoords.Z);

                        int distance =
                            HexCoordinates.HexDistance(
                            hex, centerHexCoords);
                        if ( distance > Radius)
                        {
                            continue;
                        }

                        (int x, int y) = 
                            HexCoordinates.ToOffsetCoordinates(hex);

                        InnerArea.Add((x, y));
                    }
                }
            }
        }

    }
}
