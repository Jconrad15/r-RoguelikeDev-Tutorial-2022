using System;

[Serializable]
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

    public virtual void EntityTargeted(Entity entity) { }
    public virtual void TileTargeted(Tile tile) { }
}
