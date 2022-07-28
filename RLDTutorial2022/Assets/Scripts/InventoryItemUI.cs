using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    public void Setup(Item item)
    {
        TextMeshProUGUI itemText =
            GetComponentInChildren<TextMeshProUGUI>();

        itemText.SetText(item.EntityName);
    }

}
