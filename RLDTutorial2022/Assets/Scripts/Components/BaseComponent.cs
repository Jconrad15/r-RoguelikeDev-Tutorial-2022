using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent: ICloneable
{
    public BaseComponent() { }

    protected BaseComponent(BaseComponent other) { }

    public virtual object Clone()
    {
        return new BaseComponent(this);
    }
}
