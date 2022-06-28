using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    public void CreateInitialGrid(TileGrid tileGrid)
    {
        for (int y = 0; y < tileGrid.height; y++)
        {
            for (int x = 0; x < tileGrid.width; x++)
            {
                Tile t = tileGrid.GetTileAtPos(x, y);
                if (t == null)
                {
                    Debug.LogError("Null tile");
                    continue;
                }

                DisplayTile(x, y, t);
            }
        }
    }

    private void DisplayTile(int x, int y, Tile tile)
    {
        Vector3 position;
        position.x = (x + (y * 0.5f) - (y / 2)) *
                     (HexMetrics.innerRadius * 2f);
        position.y = y * (HexMetrics.outerRadius * 1.5f);
        position.z = 0;

        GameObject tileGO = Instantiate(tilePrefab, transform);
        tileGO.transform.localPosition = position;
        
        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();
        sr.color = tile.color;
    }
}
