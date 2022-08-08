using System;

[Serializable]
public class BaseItemComponent : ICloneable
{
    [NonSerialized]
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
