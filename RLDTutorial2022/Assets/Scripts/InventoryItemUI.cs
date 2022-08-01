using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerDownHandler
{
    private Action<Item> cbScheduleToDestroy;
    private Item item;
    public void Setup(Item item)
    {
        this.item = item;
        TextMeshProUGUI itemText =
            GetComponentInChildren<TextMeshProUGUI>();

        itemText.SetText(item.ItemName);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Try use item when clicked
        if (item.TryUseItem(this))
        {
            ItemUsed();
        }
    }

    public void ItemUsedThroughTargeting() => ItemUsed();

    private void ItemUsed()
    {
        cbScheduleToDestroy?.Invoke(item);
    }

    public void RegisterScheduleToDestroy(
        Action<Item> callbackfunc)
    {
        cbScheduleToDestroy += callbackfunc;
    }

    public void UnregisterScheduleToDestroy(
        Action<Item> callbackfunc)
    {
        cbScheduleToDestroy -= callbackfunc;
    }
}
