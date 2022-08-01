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
    public void TryGetTarget(Consumable targetingItem)
    {
        StartCoroutine(Targeting(targetingItem));
    }

    private IEnumerator Targeting(Consumable targetingItem)
    {
        InterfaceLogManager.Instance.LogMessage(
                "Targeting. Left click to select - Esc to cancel.");

        // Start targeting
        playerController.StartTargeting();
        Entity targetedEntity = null;

        // Player selection of target
        while (targetedEntity == null)
        {
            // Exit if player hits escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                InterfaceLogManager.Instance.LogMessage(
                    "Targeting canceled.");
                break;
            }

            // Check for click targeting
            bool clicked = Input.GetMouseButtonDown(0);
            if (clicked)
            {
                targetedEntity = TryGetEntity();
                // Exit if entity targeted
                if (targetedEntity != null)
                {
                    InterfaceLogManager.Instance.LogMessage(
                        "Target identified.");
                }
                else
                {
                    InterfaceLogManager.Instance.LogMessage(
                        "No target present.");
                }
            }

            yield return null;
        }

        // Stop targeting
        playerController.StopTargeting();
        targetingItem.EntityTargeted(targetedEntity);
    }

    private static Entity TryGetEntity()
    {
        Entity targetedEntity;
        // Get world position under mouse click
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, 0));

        // Get tile at world position
        HexCoordinates coordinates =
            HexCoordinates.FromPosition(worldPos);
        Tile targetedTile = GameManager.Instance.Grid
            .GetTileAtHexCoords(coordinates);

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
}
