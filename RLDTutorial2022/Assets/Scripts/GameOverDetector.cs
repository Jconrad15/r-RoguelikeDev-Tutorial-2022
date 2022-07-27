using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetector : MonoBehaviour
{

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
        Debug.Log("Player Died!");
    }

}
