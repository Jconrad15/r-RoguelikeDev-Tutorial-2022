using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneBus.Instance.SetSceneBus(false);
        LoadGameScene();
    }

    public void LoadGame()
    {
        SceneBus.Instance.SetSceneBus(true);
        LoadGameScene();
    }

    public void MoveToNextLevel(int seed)
    {
        SceneBus.Instance.SetSceneBus(false, seed);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}