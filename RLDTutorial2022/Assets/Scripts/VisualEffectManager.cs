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
        PlayVisualEffect(moveVisualEffectPrefab, spawnPos);
    }

    private void OnPlayerAttack(
        Entity player, Direction direction)
    {
        Vector3 spawnPos = playerGO.transform.position;
        // Add direction vector
        Vector3 directionVector =
            direction.UnitDirection() *
            HexMetrics.innerRadius * 2;
        spawnPos += directionVector;

        PlayVisualEffect(attackVisualEffectPrefab, spawnPos);
    }

    private void PlayVisualEffect(
        VisualEffect visualEffect, Vector3 spawnPos)
    {
        // Clone prefab effect
        VisualEffect newMoveEffect = Instantiate(
            visualEffect,
            spawnPos,
            Quaternion.identity);

        // Create, play, destroy
        newMoveEffect.gameObject.SetActive(true);
        newMoveEffect.Play();
        Destroy(newMoveEffect.gameObject, 0.7f);
    }


}
