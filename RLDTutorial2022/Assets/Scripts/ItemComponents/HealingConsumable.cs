using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealingConsumable : Consumable
{

    public int Amount { get; protected set; }

    public HealingConsumable(int amount) : base()
    {
        Amount = amount;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public HealingConsumable(HealingConsumable other) : base(other)
    {
        Amount = other.Amount;
    }

    public override object Clone()
    {
        return new HealingConsumable(this);
    }

    public override bool Activate(Entity entity)
    {
        Fighter f = entity.TryGetFighterComponent();
        if (f == null)
        {
            Debug.LogError("No fighter component to heal");
            return false;
        }

        int amountRecovered = f.Heal(Amount);

        // Don't use if 0 HP recovered
        if (amountRecovered == 0)
        {
            InterfaceLogManager.Instance.LogMessage(
                "Already at full HP",
                ColorDatabase.error);
            return false;
        }

        InterfaceLogManager.Instance.LogMessage(
            entity.EntityName + " healed by " + amountRecovered,
            ColorDatabase.healthPotion);
        return true;
    }


}
