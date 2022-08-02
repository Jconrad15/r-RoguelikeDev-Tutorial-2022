using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Start()
    {
        InitializeFactories();
        _ = StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // Small time break for scripts to start
        yield return new WaitForEndOfFrame();

        // Check if load or new game
        if (SceneBus.Instance.IsLoadGame)
        {
            GameManager.Instance.LoadGame();
        }
        else
        {
            GameManager.Instance.GameStart();
        }
    }

    private static void InitializeFactories()
    {
        EntityFactory.Instance.InitializeFactory();
        ItemFactory.Instance.InitializeFactory();
    }

}
