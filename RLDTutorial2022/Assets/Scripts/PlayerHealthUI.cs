using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;

    private int maxHP;

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        Fighter f = player.TryGetFighterComponent();
        
        if (f == null)
        {
            Debug.LogError(
                "Player does not have a fighter component");
            return;
        }

        f.RegisterOnFighterHealthChanged(OnPlayerHealthChanged);
        maxHP = f.maxHP;
        OnPlayerHealthChanged(f.HP);
    }

    private void OnPlayerHealthChanged(int hp)
    {
        healthText.SetText(
            hp.ToString() + " / " + maxHP.ToString());
    }
}
