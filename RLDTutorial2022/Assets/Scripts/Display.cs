using System;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject entityPrefab;

    [SerializeField]
    private GameObject tileContainer;
    [SerializeField]
    private GameObject entityContainer;

    private List<TileGOData> tileGOData;
    private List<EntityGOData> entityGOData;

    private Action<GameObject> cbOnPlayerGOCreated;

    public void CreateInitialGrid()
    {
        TileGrid tileGrid = GameManager.Instance.Grid;
        int width = tileGrid.width;
        int height = tileGrid.height;

        tileGOData = new List<TileGOData>();
        entityGOData = new List<EntityGOData>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // For each tile in the grid
                Tile t = tileGrid.GetTileAtPos(x, y);
                if (t == null)
                {
                    Debug.LogError("Null tile");
                    continue;
                }

                CreateTileGraphic(x, y, t);

                if (t.entity != null)
                {
                    CreateEntityGraphic(x, y, t);
                }
            }
        }
    }

    private void CreateTileGraphic(int x, int y, Tile tile)
    {
        Vector3 position;
        position.x = (x + (y * 0.5f) - (y / 2)) *
                     (HexMetrics.innerRadius * 2f);
        position.y = y * (HexMetrics.outerRadius * 1.5f);
        position.z = 0;

        GameObject tileGO = Instantiate(
            tilePrefab, tileContainer.transform);
        tileGO.transform.localPosition = position;

        SetTileColor(tile, tileGO);
        tileGOData.Add(new TileGOData(tileGO, tile));

        tile.RegisterOnVisibilityChanged(OnTileVisibilityChanged);
    }

    private static void SetTileColor(Tile tile, GameObject tileGO)
    {
        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();

        if (tile.VisibilityLevel ==
            VisibilityLevel.NotVisible)
        {
            sr.color = new Color32(0, 0, 0, 255);
        }
        else if (tile.VisibilityLevel ==
            VisibilityLevel.PreviouslySeen)
        {
            sr.color = Utility.DarkenColor(tile.backgroundColor, 2);
        }
        else // Visible
        {
            sr.color = tile.backgroundColor;
        }

        tileGO.GetComponent<TileText>().SetText(tile);
    }

    private void OnTileVisibilityChanged(Tile t)
    {
        // Find gameobject for the tile
        for (int i = 0; i < tileGOData.Count; i++)
        {
            if (tileGOData[i].ContainsEntity(t))
            {
                SetTileColor(t, tileGOData[i].tileGO);
                break; // when found
            }
        }
    }

    private void CreateEntityGraphic(int x, int y, Tile tile)
    {
        Vector3 position;
        position.x = (x + (y * 0.5f) - (y / 2)) *
                     (HexMetrics.innerRadius * 2f);
        position.y = y * (HexMetrics.outerRadius * 1.5f);
        position.z = 0;

        GameObject entityGO = Instantiate(
            entityPrefab, entityContainer.transform);
        entityGO.transform.localPosition = position;

        entityGO.GetComponent<EntityText>().SetText(tile.entity);

        tile.entity.RegisterOnEntityMoved(OnEntityMoved);

        if (tile.entity.isPlayer)
        {
            cbOnPlayerGOCreated?.Invoke(entityGO);
        }

        EntityGOData data = new EntityGOData(
            entityGO, 
            tile.entity,
            entityGO.GetComponent<LerpMovement>());

        entityGOData.Add(data);
    }

    private void OnEntityMoved(Entity movedEntity)
    {
        for (int i = 0; i < entityGOData.Count; i++)
        {
            if (entityGOData[i].ContainsEntity(movedEntity))
            {
                LerpMovement lm = entityGOData[i].entityMovement;
                lm.UpdatePosition(movedEntity);
                return;
            }
        }

        Debug.LogError(
            "Moved Entity is not stored in display list");
    }

    public void RegisterOnPlayerGOCreated(
        Action<GameObject> callbackfunc)
    {
        cbOnPlayerGOCreated += callbackfunc;
    }

    public void UnregisterOnPlayerGOCreated(
        Action<GameObject> callbackfunc)
    {
        cbOnPlayerGOCreated -= callbackfunc;
    }
}
