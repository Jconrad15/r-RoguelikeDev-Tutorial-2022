using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode eastKey = KeyCode.S;
    [SerializeField]
    private KeyCode westKey = KeyCode.A;
    [SerializeField]
    private KeyCode northeastKey = KeyCode.W;
    [SerializeField]
    private KeyCode northwestKey = KeyCode.Q;
    [SerializeField]
    private KeyCode southeastKey = KeyCode.X;
    [SerializeField]
    private KeyCode southwestKey = KeyCode.Z;

    private Entity player;

    private void Start()
    {
        FindObjectOfType<EntityManager>()
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (player == null) { return; }
        CheckPlayerMovement();
    }

    private void CheckPlayerMovement()
    {
        bool west = Input.GetKeyDown(westKey);
        bool east = Input.GetKeyDown(eastKey);
        if (west || east)
        {
            // move west or east
            if (west)
            { 
                player.TryAction(Direction.W);
            }
            else
            {
                player.TryAction(Direction.E);
            }

            return;
        }

        bool northwest = Input.GetKeyDown(northwestKey);
        bool southeast = Input.GetKeyDown(southeastKey);
        if (northwest || southeast)
        {
            // move northwest or southeast
            if (northwest)
            {
                player.TryAction(Direction.NW);
            }
            else
            {
                player.TryAction(Direction.SE);
            }
            return;
        }

        bool southwest = Input.GetKeyDown(southwestKey);
        bool northeast = Input.GetKeyDown(northeastKey);
        if (southwest || northeast)
        {
            // move southwest or northeast
            if (southwest)
            {
                player.TryAction(Direction.SW);
            }
            else
            {
                player.TryAction(Direction.NE);
            }
            return;
        }
    }
}
