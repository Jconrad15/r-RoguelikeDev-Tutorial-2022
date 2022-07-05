using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerGO;
    private readonly float defaultZ = -10f;

    private void Start()
    {
        FindObjectOfType<Display>()
            .RegisterOnPlayerGOCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(GameObject playerGO)
    {
        this.playerGO = playerGO;
    }

    private void Update()
    {
        if (playerGO == null) { return; }

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 newPos = playerGO.transform.position;
        newPos.z = defaultZ;

        transform.position = newPos;
    }
}
