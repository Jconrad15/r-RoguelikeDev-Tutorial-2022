using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerGO;
    private readonly float defaultZ = -10f;
    private readonly float speed = 2f;
    private readonly float distanceThreshold = 0.01f;
    
    private readonly float maxDistance = 100f;
    private bool coroutineRunning = false;

    private void Start()
    {
        FindObjectOfType<Display>()
            .RegisterOnPlayerGOCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(GameObject playerGO)
    {
        this.playerGO = playerGO;
        transform.position = GetPlayerPos();
    }

    private void Update()
    {
        if (playerGO == null) { return; }

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 playerPos = GetPlayerPos();
        float distance = Vector3.Distance(
            transform.position, playerPos);

        if (distance > maxDistance &&
            coroutineRunning == false)
        {
            StopAllCoroutines();
            StartCoroutine(LerpToPlayerPos());
        }
    }

    private IEnumerator LerpToPlayerPos()
    {
        coroutineRunning = true;

        float distance = Vector3.Distance(
            transform.position, GetPlayerPos());

        while (distance >= distanceThreshold)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                GetPlayerPos(),
                speed * distance * Time.deltaTime);

            distance = Vector3.Distance(
                transform.position, GetPlayerPos());
            yield return null;
        }

        // set to end player pos
        transform.position = GetPlayerPos();
        coroutineRunning = false;
    }

    private Vector3 GetPlayerPos()
    {
        Vector3 playerPos = playerGO.transform.position;
        playerPos.z = defaultZ;
        return playerPos;
    }
}
