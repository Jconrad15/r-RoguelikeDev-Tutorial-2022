using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverArea;

    private void Start()
    {
        gameOverArea.SetActive(false);
    }

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        player.RegisterOnEntityDied(OnPlayerDied);
    }

    private void OnPlayerDied(Entity player)
    {
        gameOverArea.SetActive(true);
    }

}
