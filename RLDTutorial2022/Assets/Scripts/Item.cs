using System;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private Action<Item> cbOnItemPickedUp;
    private Action<Item> cbOnItemDropped;

    public string Character { get; private set; }
    public string ItemName { get; private set; }

    public Color Color { get; private set; }

    public bool BlocksMovement { get; private set; }
    public Tile CurrentTile { get; private set; }
    public Entity CurrentEntity { get; private set; }

    public List<BaseItemComponent> Components { get; private set; }

    private InventoryItemUI calledFromItemUI;

    public Item(
        string character, Color color, string itemName,
        List<BaseItemComponent> components, bool blocksMovement)
    {
        Character = character;
        Color = color;
        ItemName = itemName;
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
            else if (component is LightningDamageConsumable)
            {
                LightningDamageConsumable ldc =
                    component as LightningDamageConsumable;
                Components.Add(ldc);
                ldc.SetItem(this);
            }
            else if (component is ConfusionConsumable)
            {
                ConfusionConsumable cc =
                    component as ConfusionConsumable;
                Components.Add(cc);
                cc.SetItem(this);
            }
            else if (component is FireballDamageConsumable)
            {
                FireballDamageConsumable fdc =
                    component as FireballDamageConsumable;
                Components.Add(fdc);
                fdc.SetItem(this);
            }
        }
    }

    public Item(SavedItem savedItem, Tile tile)
    {
        Character = savedItem.character;
        Color = SavedColor.LoadToColor(savedItem.color);
        ItemName = savedItem.itemName;
        BlocksMovement = savedItem.blocksMovement;
        CurrentTile = tile;
        Components = savedItem.components;

        for (int i = 0; i < Components.Count; i++)
        {
            Components[i].SetItem(this);
        }
    }

    private static Item ItemClone(
        Item ItemToClone, Tile targetTile)
    {
        Item item = new Item(
            ItemToClone.Character,
            ItemToClone.Color,
            ItemToClone.ItemName,
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
        CurrentEntity = null;
        cbOnItemDropped?.Invoke(this);
        return true;
    }

    public void PickedUp(Entity entity)
    {
        // Remove self from tile
        CurrentTile.item = null;
        CurrentTile = null;
        CurrentEntity = entity;
        cbOnItemPickedUp?.Invoke(this);
    }

    /// <summary>
    /// Returns true if the item was used.
    /// </summary>
    /// <returns></returns>
    public bool TryUseItem(InventoryItemUI inventoryItemUI)
    {
        // Temporary storage of what UI to callback if needed
        calledFromItemUI = inventoryItemUI;

        // Try to use each component, note only uses one component
        for (int i = 0; i < Components.Count; i++)
        {
            var component = Components[i];
            
            if (component is Consumable)
            {
                Consumable c = component as Consumable;
                return c.Activate(CurrentEntity);
            }
        }

        return false;
    }

    /// <summary>
    /// Try callback to most recent stored inventory item UI,
    /// when item is used through the targeting system
    /// </summary>
    public void ItemUsedThroughTargeting()
    {
        calledFromItemUI.ItemUsedThroughTargeting();
    }

    public void Destroy()
    {
        CurrentEntity = null;

        if (CurrentTile != null)
        {
            CurrentTile.item = null;
            CurrentTile = null;
        }
        Components = null;
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
