using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FireballDamageConsumable : Consumable
{
    [JsonProperty]
    public int DamageAmount { get; protected set; }
    [JsonProperty]
    public int Radius { get; protected set; }

    [JsonConstructor]
    private FireballDamageConsumable() { }

    public FireballDamageConsumable(
        int damage) : base()
    {
        DamageAmount = damage;
        Radius = 1;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public FireballDamageConsumable(
        FireballDamageConsumable other) : base(other)
    {
        DamageAmount = other.DamageAmount;
        Radius = other.Radius;
    }

    public override object Clone()
    {
        return new FireballDamageConsumable(this);
    }

    public override bool Activate(Entity entity)
    {
        Fighter f = entity.TryGetFighterComponent();
        if (f == null)
        {
            Debug.LogError(
                "Attacker does not have fighter component");
            return false;
        }

        // Start targeting system
        TargetingSystem.Instance.TryGetTarget(this, false, Radius);
        // Return false, since this is not yet used
        return false;
    }

    /// <summary>
    /// Called when tile is targeted by player input,
    /// after activating the item.
    /// </summary>
    /// <param name="targetTile"></param>
    public override void TileTargeted(Tile targetTile)
    {
        if (targetTile == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No tile targeted",
                ColorDatabase.error);
            return;
        }
        // Fireball at the target
        // Damage at the target tile
        TryDamageEntityAtTile(targetTile);
        // Damage at neighboring tiles
        Tile[] neighbors = targetTile.GetNeighboringTiles();
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] != null)
            {
                TryDamageEntityAtTile(neighbors[i]);
            }
        }

        // If this item component is used, then notify item
        item.ItemUsedThroughTargeting();
    }

    private void TryDamageEntityAtTile(Tile tile)
    {
        Entity e = tile.entity;
        if (e != null)
        {
            Fighter f = e.TryGetFighterComponent();
            if (f != null)
            {
                InterfaceLogManager.Instance.LogMessage(
                    e.EntityName +
                    " is damaged by " +
                    DamageAmount.ToString() +
                    " from the fireball.");
                f.Damage(DamageAmount, e);
            }
        }
    }
}