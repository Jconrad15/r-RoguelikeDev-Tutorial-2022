using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public Item HealthPotionPrefab { get; private set; }
    public Item LightingScrollPrefab { get; private set; }
    public Item ConfusionScrollPrefab { get; private set; }
    public Item FireballScrollPrefab { get; private set; }

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

        LightingScrollPrefab =
            new Item(
                "~",
                ColorDatabase.yellow,
                "Lightning Scroll",
                new List<BaseItemComponent>()
                {
                    new LightningDamageConsumable(20, 5)
                },
                false);

        ConfusionScrollPrefab =
            new Item(
                "~",
                new Color32(207, 63, 255, 255),
                "Confusion Scroll",
                new List<BaseItemComponent>()
                {
                    new ConfusionConsumable(4)
                },
                false);

        FireballScrollPrefab =
            new Item(
                "~",
                ColorDatabase.fire,
                "Fireball Scroll",
                new List<BaseItemComponent>()
                {
                    new FireballDamageConsumable(12)
                },
                false);
    }

    public Item CreateProceduralScroll()
    {
        ProceduralConsumable proceduralComponent =
            new ProceduralConsumable();

        Item proceduralScroll =
            new Item(
                proceduralComponent.Character,
                proceduralComponent.ItemColor,
                proceduralComponent.ItemName,
                new List<BaseItemComponent>()
                {
                    proceduralComponent
                },
                false);

        return proceduralScroll;
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
