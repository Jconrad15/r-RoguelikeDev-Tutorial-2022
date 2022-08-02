using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hallway that is in xy offset coordinates
/// </summary>
public class Hallway
{
    private readonly float resolution = 100f;

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
        HexCoordinates a = HexCoordinates
            .FromOffsetCoordinates(startPoint.Item1, startPoint.Item2);
        HexCoordinates b = HexCoordinates
            .FromOffsetCoordinates(endPoint.Item1, endPoint.Item2);

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / resolution;
            Vector3 HexCoordPosition = new Vector3(
                Mathf.Lerp(a.X, b.X, t),
                Mathf.Lerp(a.Y, b.Y, t),
                Mathf.Lerp(a.Z, b.Z, t));

            HexCoordinates hexCoordinates1 = new HexCoordinates(
                Mathf.FloorToInt(HexCoordPosition.x),
                Mathf.FloorToInt(HexCoordPosition.z));

            HexCoordinates hexCoordinates2 = new HexCoordinates(
                Mathf.CeilToInt(HexCoordPosition.x),
                Mathf.CeilToInt(HexCoordPosition.z));

            (int, int) offsetPos1 =
                HexCoordinates.ToOffsetCoordinates(hexCoordinates1);
            (int, int) offsetPos2 =
                HexCoordinates.ToOffsetCoordinates(hexCoordinates2);

            if (hallwayPoints.Contains(offsetPos1) == false)
            {
                hallwayPoints.Add(offsetPos1);
            }
            else if (hallwayPoints.Contains(offsetPos2) == false)
            {
                hallwayPoints.Add(offsetPos2);
            }
        }

    }

    public bool Contains(int x, int y)
    {
        return hallwayPoints.Contains((x, y));
    }
}
