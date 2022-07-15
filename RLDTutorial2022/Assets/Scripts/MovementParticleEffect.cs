using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MovementParticleEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject effectGO;
    private VisualEffect effect;

    private GameObject playerGO;

    public void Initialize()
    {
        effect = effectGO.GetComponent<VisualEffect>();

        GameManager.Instance.EntityManager
            .RegisterOnPlayerCreated(OnPlayerCreated);
    
        FindObjectOfType<Display>()
                .RegisterOnPlayerGOCreated(OnPlayerGOCreated);
    }

    private void OnPlayerGOCreated(GameObject playerGO)
    {
        this.playerGO = playerGO;
        effectGO.transform.position = GetPlayerPos();
        effectGO.SetActive(true);
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
        effectGO.transform.position = GetPlayerPos();
        effect.Play();
    }

    private Vector3 GetPlayerPos()
    {
        Vector3 playerPos = playerGO.transform.position;
        playerPos.z = 0;
        return playerPos;
    }
}
