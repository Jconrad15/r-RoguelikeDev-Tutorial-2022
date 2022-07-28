using System;
using System.Collections.Generic;

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

    public void SetEntity(Entity entity) => e = entity;

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
        
        Items.Add(addItem);
        addItem.PickedUp();
        cbOnItemAdded?.Invoke(addItem);
        return true;
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
