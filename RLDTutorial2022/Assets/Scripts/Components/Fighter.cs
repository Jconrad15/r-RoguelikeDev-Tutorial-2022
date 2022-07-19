using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : BaseComponent
{
    public int maxHP;
    public int defense;
    public int power;

    private int hp;
    public int HP
    {
        get => hp;
        protected set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
        }
    }

    public Fighter(int maxHP, int defense, int power) : base()
    {
        this.maxHP = maxHP;
        this.defense = defense;
        this.power = power;
        HP = maxHP;
    }

    public Fighter(Fighter other)
    : base(other)
    {
        maxHP = other.maxHP;
        defense = other.defense;
        power = other.power;
        HP = other.maxHP;
    }

    public override object Clone()
    {
        return new Fighter(this);
    }

}
