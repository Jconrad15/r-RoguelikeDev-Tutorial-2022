using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Perk { Health, Power, Defense };
public class PerkSelectorUI : MonoBehaviour
{
    [SerializeField]
    private GameObject perkSelectionArea;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        perkSelectionArea.SetActive(true);
    }

    public void Hide()
    {
        perkSelectionArea?.SetActive(false);
    }

    public void SelectHealthPerk()
    {
        LevelUpSystem.Instance.OnPerkSelected(Perk.Health);
        Hide();
    }

    public void SelectPowerPerk()
    {
        LevelUpSystem.Instance.OnPerkSelected(Perk.Power);
        Hide();
    }

    public void SelectDefensePerk()
    {
        LevelUpSystem.Instance.OnPerkSelected(Perk.Defense);
        Hide();
    }
}
