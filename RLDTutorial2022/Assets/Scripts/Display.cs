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
    private GameObject itemPrefab;

    [SerializeField]
    private GameObject tileContainer;
    [SerializeField]
    private GameObject entityContainer;
    [SerializeField]
    private GameObject itemContainer;

    private List<TileGOData> tileGOData;
    private List<EntityGOData> entityGOData;
    private List<ItemGOData> itemGOData;

    private Action cbOnPlayerGOCreated;

    public GameObject PlayerGO { get; private set; }

    private void Start()
    {
        GameManager.Instance.RegisterOnSwitchLevel(OnSwitchLevel);
    }

    private void OnSwitchLevel()
    {
        for (int i = tileGOData.Count - 1; i >= 0; i--)
        {
            Destroy(tileGOData[i].tileGO);
        }
        tileGOData.Clear();

        for (int i = entityGOData.Count - 1; i >= 0; i--)
        {
            Destroy(entityGOData[i].entityGO);
        }
        entityGOData.Clear();

        for (int i = itemGOData.Count - 1; i >= 0; i--)
        {
            Destroy(itemGOData[i].itemGO);
        }
        itemGOData.Clear();

        Destroy(PlayerGO);
    }

    public void CreateInitialGrid()
    {
        TileGrid tileGrid = GameManager.Instance.CurrentGrid;
        int width = tileGrid.width;
        int height = tileGrid.height;

        tileGOData = new List<TileGOData>();
        entityGOData = new List<EntityGOData>();
        itemGOData = new List<ItemGOData>();

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

                if (t.item != null)
                {
                    CreateItemGraphic(x, y, t);
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
        tile.RegisterOnHighlighted(OnTileHighlighted);
        tile.RegisterOnDehighlighted(OnTileDehighlighted);
    }

    private void OnTileHighlighted(Tile tile)
    {
        // Find gameobject for the tile
        GameObject tileGO = GetTileGameObject(tile);
        if (tileGO == null) { return; }

        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();

        sr.color = ColorDatabase.highlight;
    }

    private void OnTileDehighlighted(Tile tile)
    {
        // Find gameobject for the tile
        GameObject tileGO = GetTileGameObject(tile);
        if (tileGO == null) { return; }
        SetTileColor(tile, tileGO);
    }

    private GameObject GetTileGameObject(Tile tile)
    {
        for (int i = 0; i < tileGOData.Count; i++)
        {
            if (tileGOData[i].ContainsTile(tile))
            {
                return tileGOData[i].tileGO;
            }
        }

        Debug.LogError("No gameobject for this tile");
        return null;
    }

    private static void SetTileColor(Tile tile, GameObject tileGO)
    {
        SpriteRenderer sr = tileGO.GetComponent<SpriteRenderer>();

        if (tile.VisibilityLevel ==
            VisibilityLevel.NotVisible)
        {
            sr.color = ColorDatabase.black;
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
            if (tileGOData[i].ContainsTile(t))
            {
                SetTileColor(t, tileGOData[i].tileGO);

                // if tile has an entity update entity visibility
                if (t.entity != null)
                {
                    for (int j = 0; j < entityGOData.Count; j++)
                    {
                        if (entityGOData[j].ContainsEntity(t.entity))
                        {
                            UpdateEntityVisibility(entityGOData[j]);
                            break; // when found
                        }
                    }
                }

                // if tile has an item update item visibility
                if (t.item != null)
                {
                    UpdateItemVisibility(t.item);
                }

                break; // when found
            }
        }
    }

    private void UpdateItemVisibility(Item item)
    {
        GameObject itemGO = null;

        // Find item object
        for (int j = 0; j < itemGOData.Count; j++)
        {
            if (itemGOData[j].ContainsItem(item))
            {
                itemGO = itemGOData[j].itemGO;
                break; // when found
            }
        }

        if (itemGO == null)
        {
            Debug.LogError("No itemGO");
        }

        itemGO.GetComponent<ObjectText>().SetText(item);
    }


    private void UpdateEntityVisibility(EntityGOData entityGOData)
    {
        entityGOData.entityGO.GetComponent<ObjectText>()
            .SetText(entityGOData.entity);
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

        // Setup components
        entityGO.GetComponent<ObjectText>()
            .SetText(tile.entity);
        entityGO.GetComponent<MouseOverEntity>()
            .SetEntity(tile.entity);

        // Register events
        tile.entity.RegisterOnEntityMoved(OnEntityMoved);
        tile.entity.RegisterOnEntityDied(OnEntityDied);

        if (tile.entity.IsPlayer)
        {
            PlayerGO = entityGO;
            cbOnPlayerGOCreated?.Invoke();
        }

        // Store entity with gameobject
        EntityGOData data = new EntityGOData(
            entityGO,
            tile.entity,
            entityGO.GetComponent<LerpMovement>());

        entityGOData.Add(data);
    }

    private void CreateItemGraphic(int x, int y, Tile tile)
    {
        Vector3 position;
        position.x = (x + (y * 0.5f) - (y / 2)) *
                     (HexMetrics.innerRadius * 2f);
        position.y = y * (HexMetrics.outerRadius * 1.5f);
        position.z = 0;

        GameObject itemGO = Instantiate(
            itemPrefab, itemContainer.transform);
        itemGO.transform.localPosition = position;

        // Register events
        tile.item.RegisterOnItemDropped(OnItemLocationChanged);
        tile.item.RegisterOnItemPickedUp(OnItemLocationChanged);

        // Setup components
        itemGO.GetComponent<ObjectText>()
            .SetText(tile.item);

        // Store item with gameobject
        ItemGOData data = new ItemGOData(
            itemGO,
            tile.item);

        itemGOData.Add(data);
    }

    private void OnItemLocationChanged(Item item)
    {
        UpdateItemVisibility(item);
    }

    private void OnEntityDied(Entity deadEntity)
    {
        OnEntityMoved(deadEntity);
    }

    private void OnEntityMoved(Entity movedEntity)
    {
        // When an entity moves, force tile visibility to update
        OnTileVisibilityChanged(movedEntity.CurrentTile);

        // Graphics location
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
        Action callbackfunc)
    {
        cbOnPlayerGOCreated += callbackfunc;
    }

    public void UnregisterOnPlayerGOCreated(
        Action callbackfunc)
    {
        cbOnPlayerGOCreated -= callbackfunc;
    }
}
