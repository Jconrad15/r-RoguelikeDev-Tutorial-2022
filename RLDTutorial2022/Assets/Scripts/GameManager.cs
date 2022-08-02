using System.Collections;
using System.Collections.Generic;
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
    private TileGrid[] dungeonGrids;

    public void GameStart()
    {
        Initialization();

        // Create the grid of tiles
        CurrentGrid = new TileGrid(50, 50);

        EntityManager.CreateEntities(CurrentGrid);
        itemManager.CreateItems(CurrentGrid);

        FinishGameSetup();
    }

    public void LoadGame()
    {
        SaveObject saveObject = LoadSaveGame.Load();
        Initialization();
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

    }

}
