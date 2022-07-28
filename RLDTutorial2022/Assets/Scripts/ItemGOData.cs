using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemGOData
{
    public GameObject itemGO;
    public Item item;

    public ItemGOData(GameObject itemGO, Item item)
    {
        this.itemGO = itemGO;
        this.item = item;
    }

    public bool ContainsItem(Item compareItem)
    {
        return compareItem == item;
    }
}
