using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemComponent : ICloneable
{
    public Item item;
    public void SetItem(Item item) => this.item = item;

    public BaseItemComponent() { }

    protected BaseItemComponent(BaseItemComponent other) { }

    public virtual object Clone()
    {
        return new BaseItemComponent(this);
    }

    public void Destroy()
    {
        item = null;
    }
}
