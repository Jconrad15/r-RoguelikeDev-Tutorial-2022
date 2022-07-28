using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that describes entity types.
/// </summary>
public class EntityFactory : MonoBehaviour
{
    public Entity PlayerPrefab { get; private set; }
    public Entity OrcPrefab { get; private set; }
    public Entity TrollPrefab { get; private set; }

    public void InitializeFactory()
    {
        PlayerPrefab =
            new Entity(
                "@",
                ColorDatabase.player,
                "Player",
                new List<BaseComponent>() 
                {
                    new Fighter(30, 2, 5),
                    new Inventory(5)
                },
                5,
                true,
                true);

        OrcPrefab =
            new Entity(
                "o",
                ColorDatabase.orc,
                "Orc",
                new List<BaseComponent>()
                {
                    new AI(),
                    new Fighter(10, 0, 3)
                });
        TrollPrefab =
            new Entity(
                "t",
                ColorDatabase.troll,
                "Troll",
                new List<BaseComponent>()
                {
                    new AI(),
                    new Fighter(16, 1, 4)
                });
    }

    public static EntityFactory Instance { get; private set; }
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
