using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : BaseComponent
{
    public int Capacity { get; protected set; }
    public List<Item> Items { get; protected set; }

    public Inventory() : base()
    {


    }

    public Inventory(Inventory other)
    : base(other)
    {


    }

    public void SetEntity(Entity entity) => e = entity;

    public override object Clone()
    {
        return new Inventory(this);
    }

}
