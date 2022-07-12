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
                    // Set as visible
                    grid.Tiles[i].ChangeVisibilityLevel(
                        VisibilityLevel.Visible);
                }

                i++;
            }
        }
    }


}
