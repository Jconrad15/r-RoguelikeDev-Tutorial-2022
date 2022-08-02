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

    public TileGrid Grid { get; private set; }

    public void GameStart()
    {
        // TODO: fix these calls. use event(s)

        Initialization();

        // Create the grid of tiles
        Grid = new TileGrid(50, 50);

        EntityManager.CreateEntities(Grid);
        itemManager.CreateItems(Grid);

        FinishGameSetup();
    }

    public void LoadGame()
    {
        SaveObject saveObject = LoadSaveGame.Load();
        Initialization();
        Grid = new TileGrid(saveObject);

        EntityManager.LoadEntities(Grid, saveObject);
        itemManager.LoadItems(Grid, saveObject);

        FinishGameSetup();
    }

    private void FinishGameSetup()
    {
        // Update tile graph based on entities for pathfinding
        Grid.CreateNewTileGraph();

        FindObjectOfType<Display>().CreateInitialGrid();
        TurnController.Instance.StartTurnSystem();

        InterfaceLogManager.Instance.LogMessage(
            "Welcome to Hex Caverns");
    }

    private void Initialization()
    {
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

}
