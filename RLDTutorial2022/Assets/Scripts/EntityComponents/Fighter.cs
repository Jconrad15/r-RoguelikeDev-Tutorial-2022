using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fighter : BaseComponent
{
    public int maxHP;
    public int defense;
    public int power;

    private Action<int> cbOnFighterHealthChanged;

    [JsonProperty]
    private int hp;
    public int HP
    {
        get => hp;
        protected set
        {
            hp = Mathf.Clamp(value, 0, maxHP);
            cbOnFighterHealthChanged?.Invoke(hp);
        }
    }

    public void Damage(int amount, Entity damagedBy)
    {
        HP -= amount;
        // Check if died
        if (hp <= 0) { Died(damagedBy); }
    }

    public int Heal(int amount)
    {
        if (HP == maxHP) { return 0; }

        int HPDownBy = maxHP - HP;
        HP += amount;
        // Return whichever is smaller
        return HPDownBy >= amount ? amount : HPDownBy;
    }

    private void Died(Entity killedBy)
    {
        // Give experience to killer
        Level level = e.TryGetLevelComponent();

        if (level == null)
        {
            Debug.LogError("No level component on killed entity");
            return;
        }

        int xpGiven = level.xpGivenOnKilled;

        Level killerLevel = killedBy.TryGetLevelComponent();
        killerLevel.AddXP(xpGiven);

        // This entity dies
        e.Died();
    }

    public void IncreaseMaxHP(int amount)
    {
        maxHP += amount;
        HP += amount;
    }

    public void IncreasePower(int amount)
    {
        power += amount;
    }

    public void IncreaseDefense(int amount)
    {
        defense += amount;
    }

    [JsonConstructor]
    private Fighter() { }

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
