using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LightningDamageConsumable : Consumable
{

    [JsonProperty]
    public int DamageAmount { get; protected set; }
    [JsonProperty]
    public int MaximumRange { get; protected set; }

    [JsonConstructor]
    private LightningDamageConsumable() { }

    public LightningDamageConsumable(
        int amount, int maximumRange) : base()
    {
        DamageAmount = amount;
        MaximumRange = maximumRange;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public LightningDamageConsumable(
        LightningDamageConsumable other) : base(other)
    {
        DamageAmount = other.DamageAmount;
        MaximumRange = other.MaximumRange;
    }

    public override object Clone()
    {
        return new LightningDamageConsumable(this);
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

        // Find the nearest other entity
        Entity otherEntity = GameManager.Instance
            .EntityManager.GetNearestFighterEntity(entity);

        // Don't use if not in range
        int distance = HexCoordinates.HexDistance(
            entity.CurrentTile.Coordinates,
            otherEntity.CurrentTile.Coordinates);
        if (distance > MaximumRange)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No target in range.",
                ColorDatabase.yellow);
            return false;
        }

        // Get other fighter component
        Fighter otherFighter = otherEntity.TryGetFighterComponent();
        if (otherFighter == null)
        {
            Debug.LogError(
                "Attacked entity does not have fighter component");
            return false;
        }

        // Damage other entity's fighter component
        otherFighter.Damage(DamageAmount, entity);

        InterfaceLogManager.Instance.LogMessage(
            "A lighting bolt strikes the " +
            otherEntity.EntityName +
            " with a loud thunder, for " +
            DamageAmount + " damage!",
            ColorDatabase.yellow);
        return true;
    }

}
