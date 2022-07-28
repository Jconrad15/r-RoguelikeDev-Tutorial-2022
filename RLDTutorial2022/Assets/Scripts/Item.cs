using System;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private Action<Item> cbOnItemPickedUp;
    private Action<Item> cbOnItemDropped;

    public string Character { get; private set; }
    public string EntityName { get; private set; }

    public Color Color { get; private set; }

    public bool BlocksMovement { get; private set; }
    public Tile CurrentTile { get; private set; }

    public List<BaseItemComponent> Components { get; private set; }

    public Item(
        string character, Color color, string entityName,
        List<BaseItemComponent> components, bool blocksMovement)
    {
        Character = character;
        Color = color;
        EntityName = entityName;
        BlocksMovement = blocksMovement;

        // Clone the components
        Components = new List<BaseItemComponent>();
        for (int i = 0; i < components.Count; i++)
        {
            var component = components[i].Clone();
            if (component is HealingConsumable)
            {
                HealingConsumable hc =
                    component as HealingConsumable;
                Components.Add(hc);
                hc.SetItem(this);
            }
        }
    }

    private static Item ItemClone(
        Item ItemToClone, Tile targetTile)
    {
        Item item = new Item(
            ItemToClone.Character,
            ItemToClone.Color,
            ItemToClone.EntityName,
            ItemToClone.Components,
            ItemToClone.BlocksMovement);

        item.CurrentTile = targetTile;
        targetTile.item = item;

        return item;
    }

    public static Item SpawnCloneAtTile(
        Item itemPrefab, Tile tile)
    {
        return ItemClone(itemPrefab, tile);
    }

    public bool TryPlaceAtTile(Tile targetTile)
    {
        if (targetTile == null) { return false; }
        if (targetTile.item != null) { return false; }

        targetTile.item = this;
        CurrentTile = targetTile;
        cbOnItemDropped?.Invoke(this);
        return true;
    }

    public void PickedUp()
    {
        // Remove self from tile
        CurrentTile.item = null;
        CurrentTile = null;
        cbOnItemPickedUp?.Invoke(this);
    }

    public void RegisterOnItemPickedUp(
        Action<Item> callbackfunc)
    {
        cbOnItemPickedUp += callbackfunc;
    }

    public void UnregisterOnItemPickedUp(
        Action<Item> callbackfunc)
    {
        cbOnItemPickedUp -= callbackfunc;
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
