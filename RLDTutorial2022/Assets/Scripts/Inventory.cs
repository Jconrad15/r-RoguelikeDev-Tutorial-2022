using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : BaseComponent
{
    private Action<Item> cbOnItemAdded;
    private Action<Item> cbOnItemDropped;

    public int Capacity { get; protected set; }
    public List<Item> Items { get; protected set; }

    public Inventory(int capacity) : base()
    {
        Capacity = capacity;
        Items = new List<Item>();
    }

    public Inventory(Inventory other)
    : base(other)
    {
        Capacity = other.Capacity;
        Items = other.Items;
    }

    public override object Clone()
    {
        return new Inventory(this);
    }

    public bool Drop(Item dropItem)
    {
        if (dropItem == null) { return false; }
        if (Items.Contains(dropItem) == false) { return false; }

        Tile targetTile = e.CurrentTile;

        if (dropItem.TryPlaceAtTile(targetTile) == false)
        {
            return false;
        }

        Items.Remove(dropItem);
        cbOnItemDropped?.Invoke(dropItem);
        return true;
    }

    public bool TryAddItem(Item addItem)
    {
        if (addItem == null) { return false; }
        if (Items.Count >= Capacity)
        {
            InterfaceLogManager.Instance.LogMessage(
                "Inventory is full.");
            return false;
        }
        else
        {
            InterfaceLogManager.Instance.LogMessage(
                e.EntityName + " picked up a " + 
                addItem.EntityName);
        }
        
        Items.Add(addItem);
        addItem.PickedUp(e);
        cbOnItemAdded?.Invoke(addItem);
        return true;
    }

    public void NotifyItemUsedInInventoryUI(Item usedItem)
    {
        if (Items.Contains(usedItem) == false)
        {
            Debug.LogError("Used item is not in the inventory?");
        }

        Items.Remove(usedItem);

        usedItem.Destroy();
    }

    public void RegisterOnItemAdded(
        Action<Item> callbackfunc)
    {
        cbOnItemAdded += callbackfunc;
    }

    public void UnregisterOnItemAdded(
        Action<Item> callbackfunc)
    {
        cbOnItemAdded -= callbackfunc;
    }

    public void RegisterOnItemDropped(
        Action<Item> callbackfunc)
    {
        cbOnItemDropped += callbackfunc;
    }

    public void UnregisterOnItemDropped(
        Action<Item> callbackfunc)
    {
        cbOnItemDropped -= callbackfunc;
    }

}
