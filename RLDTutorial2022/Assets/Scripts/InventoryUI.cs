using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryItemPrefab;
    [SerializeField]
    private GameObject inventoryItemContainer;

    private Dictionary<Item, GameObject> itemGOs =
        new Dictionary<Item, GameObject>();

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        Inventory inventory = player.TryGetInventoryComponent();

        if (inventory == null)
        {
            Debug.LogError(
                "Player does not have an inventory component");
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
        itemGO.GetComponent<InventoryItemUI>().Setup(item);
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
        itemGOs.Remove(item);
        Destroy(itemGO);

        Debug.Log("Item dropped from inventory");
    }

}
