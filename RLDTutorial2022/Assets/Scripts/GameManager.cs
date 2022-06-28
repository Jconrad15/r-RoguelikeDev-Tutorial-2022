using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that manages the game.
/// </summary>
public class GameManager : MonoBehaviour
{
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
        // Create the grid of tiles
        Grid = new TileGrid(6, 6);

        // TODO: fix this call. use event
        FindObjectOfType<TileGridDisplay>().CreateInitialGrid(Grid);
    }



}
