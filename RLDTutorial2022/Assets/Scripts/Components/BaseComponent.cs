using System;

public class BaseComponent: ICloneable
{
    public Entity e;

    public BaseComponent() { }

    protected BaseComponent(BaseComponent other) { }

    public virtual object Clone()
    {
        return new BaseComponent(this);
    }

    public void Destroy()
    {
        e = null;
    }
}
