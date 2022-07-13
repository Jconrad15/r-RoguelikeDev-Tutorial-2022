using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public void InitializeFOV()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        UpdateFOV(player);
        player.RegisterOnEntityMoved(OnPlayerMoved);
    }

    private void OnPlayerMoved(Entity player)
    {
        UpdateFOV(player);
    }

    public void UpdateFOV(Entity player)
    {
        TileGrid grid = GameManager.Instance.Grid;

        HexCoordinates currentHex =
            player.CurrentTile.Coordinates;

        for (int y = 0, i = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                HexCoordinates targetHex =
                    HexCoordinates.FromOffsetCoordinates(x, y);

                int dist = HexCoordinates.HexDistance(
                    targetHex, currentHex);

                if (dist > player.visibilityDistance)
                {
                    // out of range
                    if (grid.Tiles[i].VisibilityLevel ==
                        VisibilityLevel.Visible)
                    {
                        grid.Tiles[i].ChangeVisibilityLevel(
                            VisibilityLevel.PreviouslySeen);
                    }
                }
                else
                {
                    // The tile is currently in visibility range
                    // need to check if there is a blocking wall
                    // Set as visible
                    if (IsLineOfSightBlocked(currentHex, targetHex)
                        == false)
                    {
                        grid.Tiles[i].ChangeVisibilityLevel(
                            VisibilityLevel.Visible);
                    }
                    else if (grid.Tiles[i].VisibilityLevel ==
                        VisibilityLevel.Visible)
                    {
                        grid.Tiles[i].ChangeVisibilityLevel(
                            VisibilityLevel.PreviouslySeen);
                    }



                }

                i++;
            }
        }
    }

    /// <summary>
    /// Returns true if the line of sight between two hexes is blocked.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private bool IsLineOfSightBlocked(
        HexCoordinates a, HexCoordinates b)
    {
        int distance = HexCoordinates.HexDistance(a, b);
        // Neighboring and same hexes are always visible
        if (distance <= 1) { return false; }

        int resolution = distance * 10;
        // Sample points along path
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;

            float x = Mathf.Lerp(a.X, b.X, t);
            float y = Mathf.Lerp(a.Y, b.Y, t);
            float z = Mathf.Lerp(a.Z, b.Z, t);

            int ix = Mathf.RoundToInt(x);
            int iy = Mathf.RoundToInt(y);
            int iz = Mathf.RoundToInt(z);

            // Check for rounding errors
            if (ix + iy + iz != 0)
            {
                float dX = Mathf.Abs(x - ix);
                float dY = Mathf.Abs(y - iy);
                float dZ = Mathf.Abs(-x - y - iz);

                if (dX > dY && dX > dZ)
                {
                    ix = -iy - iz;
                }
                else if (dZ > dY)
                {
                    iz = -ix - iy;
                }
            }

            // Get tile at lerped position
            Tile intermediateTile = GameManager.Instance.Grid
                .GetTileAtHexCoords(new HexCoordinates(ix, iz));

            // If reach target, hex is not blocked
            if (intermediateTile.Coordinates.X == b.X &&
                intermediateTile.Coordinates.Y == b.Y &&
                intermediateTile.Coordinates.Z == b.Z)
            {
                return false;
            }

            if (intermediateTile.IsTransparent == false)
            {
                return true; // is blocked
            }

        }

        return false;
    }


}
