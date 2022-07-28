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

        itemText.SetText(item.EntityName);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Use item when clicked
        if (item.TryUseItem())
        {
            cbScheduleToDestroy?.Invoke(item);
        }
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
