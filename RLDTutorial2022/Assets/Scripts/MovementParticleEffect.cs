using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MovementParticleEffect : MonoBehaviour
{
    [SerializeField]
    private VisualEffect moveVisualEffectPrefab;

    private GameObject playerGO;

    public void Initialize()
    {
        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    
        FindObjectOfType<Display>()
                .RegisterOnPlayerGOCreated(OnPlayerGOCreated);
    }

    private void OnPlayerGOCreated(GameObject playerGO)
    {
        this.playerGO = playerGO;
    }

    private void OnPlayerCreated(Entity player)
    {
        player.RegisterOnEntityMoved(OnPlayerMoved);
    }

    private void OnPlayerMoved(Entity player)
    {
        PlayMoveParticleEffect(player);
    }

    private void PlayMoveParticleEffect(Entity player)
    {
        Vector3 spawnPos = GetPlayerPos();
        spawnPos.y -= 5f;

        // Clone prefab effect
        VisualEffect newMoveEffect = Instantiate(
            moveVisualEffectPrefab,
            spawnPos,
            Quaternion.identity);

        newMoveEffect.gameObject.SetActive(true);

        newMoveEffect.Play();

        Destroy(newMoveEffect.gameObject, 1f);
    }

    private Vector3 GetPlayerPos()
    {
        Vector3 playerPos = playerGO.transform.position;
        playerPos.z = 0;
        return playerPos;
    }
}
