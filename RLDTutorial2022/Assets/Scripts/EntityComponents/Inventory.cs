using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : BaseComponent
{
    private Action<Item> cbOnItemAdded;
    private Action<Item> cbOnItemDropped;

    public int Capacity { get; protected set; }
    [NonSerialized]
    private List<Item> items;

    public List<Item> GetItems() => items;

    [JsonConstructor]
    private Inventory() { }

    public Inventory(int capacity) : base()
    {
        Capacity = capacity;
        items = new List<Item>();
    }

    public Inventory(Inventory other)
    : base(other)
    {
        Capacity = other.Capacity;
        items = other.items;
    }

    public override object Clone()
    {
        return new Inventory(this);
    }

    public bool Drop(Item dropItem)
    {
        if (dropItem == null) { return false; }
        if (items.Contains(dropItem) == false) { return false; }

        Tile targetTile = e.CurrentTile;

        if (dropItem.TryPlaceAtTile(targetTile) == false)
        {
            return false;
        }

        items.Remove(dropItem);
        cbOnItemDropped?.Invoke(dropItem);
        return true;
    }

    public bool TryAddItem(Item addItem)
    {
        if (addItem == null) { return false; }
        if (items.Count >= Capacity)
        {
            InterfaceLogManager.Instance.LogMessage(
                "Inventory is full.");
            return false;
        }
        else
        {
            InterfaceLogManager.Instance.LogMessage(
                e.EntityName + " picked up a " + 
                addItem.ItemName);
        }
        
        items.Add(addItem);
        addItem.PickedUp(e);
        cbOnItemAdded?.Invoke(addItem);
        return true;
    }

    public void NotifyItemUsedInInventoryUI(Item usedItem)
    {
        if (items.Contains(usedItem) == false)
        {
            Debug.LogError("Used item is not in the inventory?");
        }

        items.Remove(usedItem);

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
