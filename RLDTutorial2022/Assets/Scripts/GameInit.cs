using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public void StartButton()
    {
        InitializeFactories();
        GameManager.Instance.GameStart();
    }

    private static void InitializeFactories()
    {
        EntityFactory.Instance.InitializeFactory();
        ItemFactory.Instance.InitializeFactory();
    }

    public void LoadButton()
    {
        InitializeFactories();
        GameManager.Instance.LoadGame();
    }

}
