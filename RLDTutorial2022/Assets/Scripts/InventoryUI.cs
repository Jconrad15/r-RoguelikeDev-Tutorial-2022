using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField]
    private GameObject inventoryItemPrefab;
    [SerializeField]
    private GameObject inventoryArea;
    [SerializeField]
    private GameObject inventoryItemContainer;

    private Dictionary<Item, GameObject> itemGOs =
        new Dictionary<Item, GameObject>();

    private bool isHidden;

    private void Update()
    {
        // player actions to open inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (isHidden)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        inventoryArea.SetActive(true);
        isHidden = false;
    }

    private void Hide()
    {
        inventoryArea.SetActive(false);
        isHidden = true;
    }

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
        Hide();
    }

    private void OnPlayerCreated(Entity player)
    {
        inventory = player.TryGetInventoryComponent();

        if (inventory == null)
        {
            Debug.LogError(
                "Player does not have an inventory component");
            return;
        }

        inventory.RegisterOnItemDropped(OnItemDropped);
        inventory.RegisterOnItemAdded(OnItemAdded);
    }

    private void OnItemAdded(Item item)
    {
        GameObject itemGO = CreateInventoryItemGO(item);
        itemGOs.Add(item, itemGO);
    }

    private GameObject CreateInventoryItemGO(Item item)
    {
        GameObject itemGO = Instantiate(
            inventoryItemPrefab, inventoryItemContainer.transform);
        InventoryItemUI inventoryItemUI =
            itemGO.GetComponent<InventoryItemUI>();

        inventoryItemUI.Setup(item);
        inventoryItemUI.RegisterScheduleToDestroy(
            UsedInventoryItemUI);

        return itemGO;
    }

    private void OnItemDropped(Item item)
    {
        if (itemGOs.ContainsKey(item) == false)
        {
            Debug.LogError("Removing an item that was" +
                " not displayed in inventory UI??");
            return;
        }

        GameObject itemGO = itemGOs[item];
        _ = itemGOs.Remove(item);
        Destroy(itemGO);
    }

    private void UsedInventoryItemUI(Item item)
    {
        if (itemGOs.ContainsKey(item) == false)
        {
            Debug.LogError("Removing an item that was" +
                " not displayed in inventory UI??");
            return;
        }

        GameObject itemGO = itemGOs[item];
        _ = itemGOs.Remove(item);
        Destroy(itemGO);

        inventory.NotifyItemUsedInInventoryUI(item);
    }
}
