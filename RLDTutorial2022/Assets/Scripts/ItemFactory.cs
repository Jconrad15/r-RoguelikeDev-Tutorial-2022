using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public Item HealthPotionPrefab { get; private set; }

    public void InitializeFactory()
    {
        HealthPotionPrefab =
            new Item(
                "!",
                ColorDatabase.healthPotion,
                "Health Potion",
                new List<BaseItemComponent>()
                {
                    new HealingConsumable(4)
                },
                false);
    }

    public static ItemFactory Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
