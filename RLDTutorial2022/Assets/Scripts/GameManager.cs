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
    public Dungeon CurrentDungeon { get; private set; }

    /// <summary>
    /// Starts a new game or a new level.
    /// </summary>
    public void GameStart()
    {
        Initialization();

        // Check if a seed was stored in SceneBus for current level
        if (SceneBus.Instance.MoveToSeed != int.MaxValue)
        {
            CreateNextLevel();
        }
        else
        {
            CreateDungeon();
        }

        FinishGameSetup();
    }

    private void CreateDungeon()
    {
        // Otherwise create dungeon and use the first seed
        CurrentDungeon = Dungeon.RandomDungeon();
        CurrentGridSeed = CurrentDungeon.dungeonGridSeeds[0];

        // Create the grid of tiles
        CurrentGrid = new TileGrid(50, 50, CurrentGridSeed);
        EntityManager.CreateEntities(
            CurrentGrid,
            CurrentGridSeed);

        itemManager.CreateItems(
            CurrentGrid,
            CurrentGridSeed);
    }

    private void CreateNextLevel()
    {
        // Use stored dungeon and seed
        CurrentDungeon = SceneBus.Instance.currentDungeon;
        CurrentGridSeed = SceneBus.Instance.MoveToSeed;

        // Create the grid of tiles
        CurrentGrid = new TileGrid(50, 50, CurrentGridSeed);
        EntityManager.CreateEntities(
            CurrentGrid,
            CurrentGridSeed,
            SceneBus.Instance.PlayerEntity);

        itemManager.CreateItems(
            CurrentGrid,
            CurrentGridSeed);

        SceneBus.Instance.Clear();
    }

    /// <summary>
    /// Loads a game from disk.
    /// </summary>
    public void LoadGame()
    {
        SaveObject saveObject = LoadSaveGame.Load();

        Initialization();

        CurrentDungeon = new Dungeon(saveObject.dungeonGridSeeds);
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
            "Welcome to Hex Caverns Level " +
            (GetCurrentSeedIndex() + 1).ToString() + ".");
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
        SwitchToNextLevel();
    }

    private void SwitchToNextLevel()
    {
        SwitchToNextSeed();

        // Set sceneBus data
        SceneBus.Instance.currentDungeon = CurrentDungeon;
        SceneBus.Instance.SetPlayerEntity(
            EntityManager.GetPlayerEntity());

        FindObjectOfType<SceneChanger>()
            .MoveToNextLevel(CurrentGridSeed);
    }

    private void SwitchToNextSeed()
    {
        int currentIndex = GetCurrentSeedIndex();

        int nextIndex = currentIndex + 1;
        if (nextIndex >= CurrentDungeon.dungeonGridSeeds.Length)
        {
            // Switch to victory menu if no more levels
            FindObjectOfType<SceneChanger>().LoadVictoryMenu();
            return;
        }

        CurrentGridSeed = CurrentDungeon.dungeonGridSeeds[nextIndex];
    }

    private int GetCurrentSeedIndex()
    {
        return Array.IndexOf(
            CurrentDungeon.dungeonGridSeeds,
            CurrentGridSeed);
    }
}
