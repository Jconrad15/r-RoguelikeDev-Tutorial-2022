using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerGO;
    private readonly float defaultZ = -10f;
    private readonly float speed = 2f;
    private readonly float distanceThreshold = 0.01f;
    
    private readonly float maxDistance = 70f;
    private bool coroutineRunning = false;

    private Display display;

    private void Start()
    {
        display = FindObjectOfType<Display>();
        display.RegisterOnPlayerGOCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated()
    {
        playerGO = display.PlayerGO;
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

            // Lerp quicker if near end
            if (distance < 5f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    GetPlayerPos(),
                    speed * Time.deltaTime);
            }

            distance = Vector3.Distance(
                transform.position, GetPlayerPos());

            yield return null;
        }

        // Set to end player pos
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
