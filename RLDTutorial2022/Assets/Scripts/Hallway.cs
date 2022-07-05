using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hallway that is in xy offset coordinates
/// </summary>
public class Hallway
{
    private readonly float resolution = 1000f;

    public (int, int) startPoint { get; private set; }
    public (int, int) endPoint { get; private set; }

    public List<(int, int)> hallwayPoints { get; private set; }

    public Hallway((int, int) startPoint, (int, int) endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;

        CreateHallwayPoints();
    }

    private void CreateHallwayPoints()
    {
        hallwayPoints = new List<(int, int)>
        {
            startPoint,
            endPoint
        };

        // Create list of points betwen start and end points

        Vector2 a = new Vector2(startPoint.Item1, startPoint.Item2);
        Vector2 b = new Vector2(endPoint.Item1, endPoint.Item2);
        for (int i = 0; i < resolution; i++)
        {
            Vector2 vectorPoint = Vector2.Lerp(a, b, i / resolution);
            (int, int) tuplePoint =
                (Mathf.RoundToInt(vectorPoint.x),
                Mathf.RoundToInt(vectorPoint.y));

            if (hallwayPoints.Contains(tuplePoint) == false)
            {
                hallwayPoints.Add(tuplePoint);
            }
        }

    }

    public bool Contains(int x, int y)
    {
        return hallwayPoints.Contains((x, y));
    }
}
