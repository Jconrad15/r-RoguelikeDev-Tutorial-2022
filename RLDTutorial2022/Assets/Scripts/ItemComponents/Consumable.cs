using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : BaseItemComponent
{

    public Consumable() : base() { }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public Consumable(Consumable other) : base(other) { }

    public override object Clone()
    {
        return new Consumable(this);
    }

    public virtual bool Activate(Entity entity) { return false; }

}
