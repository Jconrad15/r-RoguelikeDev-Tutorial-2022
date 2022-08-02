using System;

[Serializable]
public class BaseComponent : ICloneable
{
    [NonSerialized]
    protected Entity e;

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

    public void SetEntity(Entity entity) => e = entity;

}
