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

    private List<GameObject> createdTileGOs;
    private List<GameObject> createdEntityGOs;
    private List<Entity> entities;

    private Action<GameObject> cbOnPlayerGOCreated;

    public void CreateInitialGrid(TileGrid tileGrid)
    {
        int width = tileGrid.width;
        int height = tileGrid.height;

        createdTileGOs = new List<GameObject>();
        createdEntityGOs = new List<GameObject>();
        entities = new List<Entity>();

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
        
        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();
        sr.color = tile.backgroundColor;

        tileGO.GetComponent<TileText>().SetText(tile);

        createdTileGOs.Add(tileGO);
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

        createdEntityGOs.Add(entityGO);

        tile.entity.RegisterOnEntityMoved(OnEntityMoved);
        entities.Add(tile.entity);

        if (tile.entity.isPlayer)
        {
            cbOnPlayerGOCreated?.Invoke(entityGO);
        }
    }

    private void OnEntityMoved(Entity movedEntity)
    {
        int index = entities.FindIndex(a => a == movedEntity);

        if (index == -1)
        {
            Debug.LogError("Moved Entity is not stored in display list");
            return;
        }

        UpdatePosition(createdEntityGOs[index], movedEntity);
    }

    private void UpdatePosition(GameObject entityGO, Entity entity)
    {
        HexCoordinates hexCoords = entity.CurrentTile.Coordinates;
        Vector3 position = hexCoords.GetWorldPosition();

        entityGO.transform.position = position;
    }

    public void RegisterOnPlayerGOCreated(Action<GameObject> callbackfunc)
    {
        cbOnPlayerGOCreated += callbackfunc;
    }

    public void UnregisterOnPlayerGOCreated(Action<GameObject> callbackfunc)
    {
        cbOnPlayerGOCreated -= callbackfunc;
    }
}
