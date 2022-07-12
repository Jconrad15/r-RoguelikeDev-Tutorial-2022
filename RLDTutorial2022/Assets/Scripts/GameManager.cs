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
        // TODO: fix these calls. use event(s)

        EntityManager = FindObjectOfType<EntityManager>();
        FindObjectOfType<FieldOfView>().InitializeFOV();

        // Create the grid of tiles
        Grid = new TileGrid(50, 50);

        // Create the player
        EntityManager.CreatePlayer(
            Grid.GetRandomRoomCenterTile(), playerColor);

        FindObjectOfType<Display>().CreateInitialGrid();
    }

}
