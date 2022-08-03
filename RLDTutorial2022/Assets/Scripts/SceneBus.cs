using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBus : MonoBehaviour
{
    public bool IsLoadGame { get; private set; }
    public int MoveToSeed { get; private set; }
    public Dungeon currentDungeon;
    public Entity PlayerEntity { get; private set; }
    public List<Item> PlayerInventoryItems {  get; private set; }

    public static SceneBus Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSceneBus(
        bool isLoadGame, int seed = int.MaxValue)
    {
        IsLoadGame = isLoadGame;
        MoveToSeed = seed;
    }

    public void SetPlayerEntity(Entity player)
    {
        PlayerEntity = player;
        PlayerEntity.ClearCallbacks();

        PlayerInventoryItems =
            PlayerEntity.TryGetInventoryComponent().GetItems();
    }

    public void Clear()
    {
        IsLoadGame = false;
        MoveToSeed = int.MaxValue;
        PlayerEntity = null;
        currentDungeon = new Dungeon();
    }

}