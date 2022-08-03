using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : BaseComponent
{
    public int level = 1;
    public int xp = 0;
    public int levelUpBase = 0;
    public int levelUpFactor = 150;
    public int xpGiven = 0;

    [JsonConstructor]
    private Level() { }

    public Level(int level = 1, int xp = 0, int levelUpBase = 0,
        int levelUpFactor = 150, int xpGiven = 0) : base()
    {
        this.level = level;
        this.xp = xp;
        this.levelUpBase = levelUpBase;
        this.levelUpFactor = levelUpFactor;
        this.xpGiven = xpGiven;
    }

    public Level(Level other)
    : base(other)
    {

    }

    public override object Clone()
    {
        return new Level(this);
    }

    public int ExperienceToNextLevel()
    {
        return levelUpBase + (level * levelUpFactor);
    }

    public bool CheckRequireLevelUp()
    {
        return xp > ExperienceToNextLevel();
    }

    public void AddXP(int amount)
    {
        if (amount == 0 || levelUpBase == 0) { return; }

        xp += amount;
        InterfaceLogManager.Instance.LogMessage(
            "You gain " + amount + " experience points.");

        if (CheckRequireLevelUp())
        {
            InterfaceLogManager.Instance.LogMessage(
                "You advance to level " + 
                (level + 1).ToString() + ".");
        }
    }

    private void IncreaseLevel()
    {
        xp -= ExperienceToNextLevel();
        level++;
    }

    public void IncreaseMaxHP(int amount = 20)
    {
        Fighter f = e.TryGetFighterComponent();
        f.IncreaseMaxHP(amount);

        InterfaceLogManager.Instance.LogMessage(
                "Your health improves!");
        IncreaseLevel();
    }

    public void IncreasePower(int amount = 1)
    {
        Fighter f = e.TryGetFighterComponent();
        f.IncreasePower(amount);

        InterfaceLogManager.Instance.LogMessage(
                "You feel stronger!");
        IncreaseLevel();
    }

    public void IncreaseDefense(int amount = 1)
    {
        Fighter f = e.TryGetFighterComponent();
        f.IncreaseDefense(amount);

        InterfaceLogManager.Instance.LogMessage(
                "Your movements are getting swifter!");
        IncreaseLevel();
    }

}
