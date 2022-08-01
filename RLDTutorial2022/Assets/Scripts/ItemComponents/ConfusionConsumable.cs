using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfusionConsumable : Consumable
{
    private Entity currentEntity;
    public int NumberOfTurns { get; protected set; }
    public ConfusionConsumable(
        int numberOfTurns) : base()
    {
        NumberOfTurns = numberOfTurns;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public ConfusionConsumable(
        ConfusionConsumable other) : base(other)
    {
        NumberOfTurns = other.NumberOfTurns;
    }

    public override object Clone()
    {
        return new ConfusionConsumable(this);
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
        currentEntity = entity;

        // Start targeting system
        TargetingSystem.Instance.TryGetTarget(this);
        // Return false, since this is not yet used
        return false;
    }

    /// <summary>
    /// Called when entity is targeted by player input,
    /// after activating the item.
    /// </summary>
    /// <param name="targetEntity"></param>
    public override void EntityTargeted(Entity targetEntity)
    {
        if (targetEntity == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No entity targeted",
                ColorDatabase.error);
            return;
        }

        if (targetEntity == currentEntity)
        {
            InterfaceLogManager.Instance.LogMessage(
                "Cannot confuse yourself",
                ColorDatabase.error);
            return;
        }

        // Confuse the target
        targetEntity.TryGetAIComponent().ConfuseAI(NumberOfTurns);
        InterfaceLogManager.Instance.LogMessage(
                targetEntity.EntityName + " is confused for " +
                NumberOfTurns.ToString() + " turns.");

        // If this item component is used, then notify item
        item.ItemUsedThroughTargeting();
        currentEntity = null;
    }

}
