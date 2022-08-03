using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that aids in player selecting targets.
/// </summary>
public class TargetingSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private List<Tile> highlightedTiles;

    private Action<bool> cbOnTargetingStatusChanged;

    public static TargetingSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void TryGetTarget(
        Consumable targetingItem, bool isTargetingEntity = true,
        int radius = 0)
    {
        StartCoroutine(Targeting(
            targetingItem, isTargetingEntity, radius));
    }

    private IEnumerator Targeting(
        Consumable targetingItem, bool isTargetingEntity,
        int radius)
    {
        // Wait for end of frame, so that previous
        // clicks don't trigger location selection
        yield return new WaitForEndOfFrame();

        // Start targeting
        TargetingMessage(isTargetingEntity);
        playerController.StartModal();
        cbOnTargetingStatusChanged?.Invoke(true);
        Entity targetedEntity = null;
        Tile targetedTile = null;

        highlightedTiles = new List<Tile>();

        bool targetFound = false;
        // Player selection of target
        while (targetFound == false)
        {
            // Exit if player hits escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                InterfaceLogManager.Instance.LogMessage(
                    "Targeting canceled.");
                break;
            }

            // Display targeting area
            DetermineHighlighting(radius);

            CheckForClick(isTargetingEntity, ref targetedEntity,
                ref targetedTile, ref targetFound);

            yield return null;
        } // end while

        // dehighlight all
        DehighlightAll();

        // Stop targeting
        playerController.StopModal();
        cbOnTargetingStatusChanged?.Invoke(false);
        if (isTargetingEntity)
        {
            targetingItem.EntityTargeted(targetedEntity);
        }
        else
        {
            targetingItem.TileTargeted(targetedTile);
        }
    }

    private void DetermineHighlighting(
        int radius)
    {
        List<Tile> hoveredTiles = new List<Tile>();
        Tile mainHoveredTile = TryGetTile();
        hoveredTiles.Add(mainHoveredTile);

        // TODO: better method of getting surrounding tiles
        if (radius == 1)
        {
            Tile[] neighbors = mainHoveredTile.GetNeighboringTiles();
            foreach (Tile neighbor in neighbors)
            {
                if (neighbor == null) { continue; }
                hoveredTiles.Add(neighbor);
            }
        }

        // highlight
        for (int i = 0; i < hoveredTiles.Count; i++)
        {
            // if hovered tile is not highlighted
            if (highlightedTiles.Contains(hoveredTiles[i]) ==
                false)
            {
                hoveredTiles[i].Highlight();
                highlightedTiles.Add(hoveredTiles[i]);
            }
        }
        // dehighlight
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            // if highlighted tile is not hovered
            if (hoveredTiles.Contains(highlightedTiles[i]) ==
                false)
            {
                highlightedTiles[i].Dehighlight();
                highlightedTiles.Remove(highlightedTiles[i]);
            }
        }
    }

    private void DehighlightAll()
    {
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            highlightedTiles[i].Dehighlight();
        }
        highlightedTiles = null;
    }

    private static void CheckForClick(
        bool isTargetingEntity, ref Entity targetedEntity,
        ref Tile targetedTile, ref bool targetFound)
    {
        bool clicked = Input.GetMouseButtonDown(0);
        if (clicked)
        {
            if (isTargetingEntity)
            {
                targetedEntity = TryGetEntity();
            }
            else
            {
                targetedTile = TryGetTile();
            }

            // Exit if entity targeted
            if (targetedEntity != null ||
                targetedTile != null)
            {
                InterfaceLogManager.Instance.LogMessage(
                    "Target identified.");
                targetFound = true;
            }
            else
            {
                InterfaceLogManager.Instance.LogMessage(
                    "No target present.");
            }
        }
    }

    private static Entity TryGetEntity()
    {
        Entity targetedEntity;
        Tile targetedTile = TryGetTile();

        if (targetedTile == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "Null tile targeted.");
            return null;
        }

        // Get entity at tile
        targetedEntity = targetedTile.entity;
        return targetedEntity;
    }

    private static Tile TryGetTile()
    {
        Tile targetedTile;
        // Get world position under mouse click
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, 0));

        // Get tile at world position
        HexCoordinates coordinates =
            HexCoordinates.FromPosition(worldPos);
        targetedTile = GameManager.Instance.CurrentGrid
            .GetTileAtHexCoords(coordinates);

        return targetedTile;
    }

    private static void TargetingMessage(bool isTargetingEntity)
    {
        string targetType = isTargetingEntity ? "entity" : "tile";
        InterfaceLogManager.Instance.LogMessage(
            "Targeting " + targetType +
            ". Left click to select - Esc to cancel.");
    }

    public void RegisterOnTargetingStatusChanged(
        Action<bool> callbackfunc)
    {
        cbOnTargetingStatusChanged += callbackfunc;
    }

    public void UnregisterOnTargetingStatusChanged(
        Action<bool> callbackfunc)
    {
        cbOnTargetingStatusChanged -= callbackfunc;
    }
}
