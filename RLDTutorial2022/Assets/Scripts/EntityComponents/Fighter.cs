using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : BaseComponent
{
    public int maxHP;
    public int defense;
    public int power;

    private Action<int> cbOnFighterHealthChanged; 

    private int hp;
    public int HP
    {
        get => hp;
        protected set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
            cbOnFighterHealthChanged?.Invoke(hp);
            if (hp <= 0)
            {
                Died();
            }
        }
    }

    public void Damage(int amount)
    {
        HP -= amount;
    }

    public int Heal(int amount)
    {
        if (HP == maxHP) { return 0; }

        int HPDownBy = maxHP - HP;
        HP += amount;
        // Return whichever is smaller
        return HPDownBy >= amount ? amount : HPDownBy;
    }

    private void Died()
    {
        e.Died();
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

    public void SetEntity(Entity entity) => e = entity;

    public override object Clone()
    {
        return new Fighter(this);
    }

    public void RegisterOnFighterHealthChanged(
        Action<int> callbackfunc)
    {
        cbOnFighterHealthChanged += callbackfunc;
    }

    public void UnregisterOnFighterHealthChanged(
        Action<int> callbackfunc)
    {
        cbOnFighterHealthChanged -= callbackfunc;
    }
}
