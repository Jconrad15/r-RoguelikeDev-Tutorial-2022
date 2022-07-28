using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
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



}
