using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Entity player;

    private void Start()
    {
        FindObjectOfType<EntityManager>()
            .RegisterOnPlayerCreated(OnPlayerCreated);
    }

    private void OnPlayerCreated(Entity player)
    {
        Debug.Log("Player set");
        this.player = player;
    }

    private void Update()
    {
        if (player == null) { return; }
        CheckPlayerMovement();
    }

    private void CheckPlayerMovement()
    {
        bool west = Input.GetKeyDown(KeyCode.A);
        bool east = Input.GetKeyDown(KeyCode.S);
        if (west || east)
        {
            // move west or east
            if (west) 
            { 
                player.TryMove(Direction.W);
            }
            else
            {
                player.TryMove(Direction.E);
            }

            return;
        }

        bool northwest = Input.GetKeyDown(KeyCode.Q);
        bool southeast = Input.GetKeyDown(KeyCode.X);
        if (northwest || southeast)
        {
            // move northwest or southeast
            if (northwest)
            {
                player.TryMove(Direction.NW);
            }
            else
            {
                player.TryMove(Direction.SE);
            }
            return;
        }

        bool southwest = Input.GetKeyDown(KeyCode.Z);
        bool northeast = Input.GetKeyDown(KeyCode.W);
        if (southwest || northeast)
        {
            // move southwest or northeast
            if (southwest)
            {
                player.TryMove(Direction.SW);
            }
            else
            {
                player.TryMove(Direction.NE);
            }
            return;
        }
    }
}
