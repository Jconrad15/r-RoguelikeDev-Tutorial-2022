using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public EntityManager EntityManager { get; private set; }

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

    public void GameStart(Color playerColor)
    {
        EntityManager = FindObjectOfType<EntityManager>();

        // Create the grid of tiles
        Grid = new TileGrid(6, 6);

        // Create the player at ltile 0
        EntityManager.CreatePlayer(Grid.Tiles[0], playerColor);

        // TODO: fix this call. use event
        FindObjectOfType<Display>().CreateInitialGrid(Grid);


    }



}
