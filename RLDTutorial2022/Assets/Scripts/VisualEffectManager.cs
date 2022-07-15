using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectManager : MonoBehaviour
{
    [SerializeField]
    private VisualEffect moveVisualEffectPrefab;
    [SerializeField]
    private VisualEffect attackVisualEffectPrefab;

    private Display display;
    private GameObject playerGO;

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);

        display = FindObjectOfType<Display>();
        display.RegisterOnPlayerGOCreated(OnPlayerGOCreated);
    }

    /// <summary>
    /// Store reference to playerGO object
    /// </summary>
    /// <param name="playerGO"></param>
    private void OnPlayerGOCreated()
    {
        playerGO = display.PlayerGO;
    }

    private void OnPlayerCreated(Entity player)
    {
        player.RegisterOnEntityMoved(OnPlayerMoved);
        player.RegisterOnEntityAttack(OnPlayerAttack);
    }

    private void OnPlayerMoved(Entity player)
    {
        Vector3 spawnPos = playerGO.transform.position;
        spawnPos.y -= 6f;
        PlayVisualEffect(
            moveVisualEffectPrefab,
            spawnPos,
            Vector2.zero);
    }

    private void OnPlayerAttack(
        Entity player, Direction direction)
    {
        Vector3 spawnPos = playerGO.transform.position;
        // Add direction vector
        Vector3 directionVector = direction.UnitDirection();

        spawnPos += HexMetrics.innerRadius * directionVector;

        PlayVisualEffect(
            attackVisualEffectPrefab,
            spawnPos,
            directionVector);
    }

    private void PlayVisualEffect(
        VisualEffect visualEffect, Vector3 spawnPos,
        Vector2 direction)
    {
        // Clone prefab effect
        VisualEffect spawnedEffect = Instantiate(
            visualEffect,
            spawnPos,
            Quaternion.identity);

        spawnedEffect.SetVector2("_direction", direction);


        // Create, play, destroy
        spawnedEffect.gameObject.SetActive(true);
        spawnedEffect.Play();
        Destroy(spawnedEffect.gameObject, 0.7f);
    }


}
