using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// Singleton that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public EntityManager EntityManager { get; private set; }
    private ItemManager itemManager;

    private Action cbOnSwitchLevel;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public TileGrid CurrentGrid { get; private set; }
    public int CurrentGridSeed { get; private set; }
    public int[] DungeonGridSeeds { get; private set; }

    public void GameStart()
    {
        Initialization();

        // Create dungeon
        DungeonGridSeeds = new int[3];
        DungeonGridSeeds[0] = 0;
        DungeonGridSeeds[1] = 2;
        DungeonGridSeeds[2] = 8;

        CurrentGridSeed = DungeonGridSeeds[0];

        // Create the grid of tiles
        CurrentGrid = new TileGrid(50, 50, CurrentGridSeed);

        EntityManager.CreateEntities(CurrentGrid, CurrentGridSeed);
        itemManager.CreateItems(CurrentGrid, CurrentGridSeed);

        FinishGameSetup();
    }

    public void LoadGame()
    {
        SaveObject saveObject = LoadSaveGame.Load();
        Initialization();
        DungeonGridSeeds = saveObject.dungeonGridSeeds;
        CurrentGridSeed = saveObject.currentGridSeed;
        CurrentGrid = new TileGrid(saveObject);

        EntityManager.LoadEntities(CurrentGrid, saveObject);
        itemManager.LoadItems(CurrentGrid, saveObject);

        FinishGameSetup();
    }

    private void FinishGameSetup()
    {
        // Update tile graph based on entities for pathfinding
        CurrentGrid.CreateNewTileGraph();

        FindObjectOfType<Display>().CreateInitialGrid();
        TurnController.Instance.StartTurnSystem();

        InterfaceLogManager.Instance.LogMessage(
            "Welcome to Hex Caverns");
    }

    private void Initialization()
    {
        // TODO: fix these calls. use event(s)
        EntityManager = FindObjectOfType<EntityManager>();
        itemManager = FindObjectOfType<ItemManager>();
        FindObjectOfType<FieldOfView>().InitializeFOV();
        FindObjectOfType<VisualEffectManager>().Initialize();
        FindObjectOfType<SoundManager>().Initialize();
        FindObjectOfType<GameOverDetector>().Initialize();
        InterfaceLogManager.Instance.Initialize();
        FindObjectOfType<PlayerHealthUI>().Initialize();
        FindObjectOfType<InventoryUI>().Initialize();
    }

    public void GoDownStairs()
    {
        _ = StartCoroutine(SwitchToNextLevel());
    }

    private IEnumerator SwitchToNextLevel()
    {
        Debug.Log("Go down stairs");
        // Need to clean up everything
        cbOnSwitchLevel?.Invoke();
        CurrentGrid.Destroy();

        // Wait for everything to clean up
        yield return new WaitForSeconds(1f);

        // Create new tileGrid
        SwitchToNextSeed();
        CurrentGrid = new TileGrid(50, 50, CurrentGridSeed);

        EntityManager.CreateEntities(CurrentGrid, CurrentGridSeed);
        itemManager.CreateItems(CurrentGrid, CurrentGridSeed);

        FinishGameSetup();
    }

    private void SwitchToNextSeed()
    {
        int currentIndex = Array.IndexOf(
            DungeonGridSeeds, CurrentGridSeed);

        int nextIndex = currentIndex + 1;
        if (nextIndex >= DungeonGridSeeds.Length)
        {
            Debug.Log("You reached the end!");
            return;
        }

        CurrentGridSeed = DungeonGridSeeds[nextIndex];
    }

    public void RegisterOnSwitchLevel(
        Action callbackfunc)
    {
        cbOnSwitchLevel += callbackfunc;
    }

    public void UnregisterOnSwitchLevel(
        Action callbackfunc)
    {
        cbOnSwitchLevel -= callbackfunc;
    }
}
