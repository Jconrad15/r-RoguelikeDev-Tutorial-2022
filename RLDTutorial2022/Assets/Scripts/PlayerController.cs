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
    [SerializeField]
    private KeyCode pickUpItemKey = KeyCode.G;

    private Entity player;

    private void Start()
    {
        FindObjectOfType<EntityManager>()
            .RegisterOnPlayerCreated(OnPlayerCreated);

        TurnController.Instance
            .RegisterOnStartPlayerTurn(OnStartTurn);
    }

    private void OnPlayerCreated(Entity player)
    {
        this.player = player;
    }

    private void OnStartTurn()
    {
        if (player == null) 
        {
            Debug.LogError("No player entity");
            // Turn is done
            TurnController.Instance.NextTurn();
            return;
        }

        StartCoroutine(PlayerProcessing());
    }

    private IEnumerator PlayerProcessing()
    {
        // Leave loop and end turn when player moves
        bool playerActed = false;
        while (playerActed == false)
        {
            playerActed = CheckPlayerAction();

            yield return null;
        }

        // Turn is done
        TurnController.Instance.NextTurn();
    }

    private bool CheckPlayerAction()
    {
        if (Input.GetKeyDown(pickUpItemKey))
        {
            return player.TryPickUpItem();
        }


        bool west = Input.GetKeyDown(westKey);
        bool east = Input.GetKeyDown(eastKey);

        if (west || east)
        {
            // move west or east
            if (west)
            { 
                return player.TryAction(Direction.W);
            }
            else
            {
                return player.TryAction(Direction.E);
            }
        }

        bool northwest = Input.GetKeyDown(northwestKey);
        bool southeast = Input.GetKeyDown(southeastKey);
        if (northwest || southeast)
        {
            // move northwest or southeast
            if (northwest)
            {
                return player.TryAction(Direction.NW);
            }
            else
            {
                return player.TryAction(Direction.SE);
            }
        }

        bool southwest = Input.GetKeyDown(southwestKey);
        bool northeast = Input.GetKeyDown(northeastKey);
        if (southwest || northeast)
        {
            // move southwest or northeast
            if (southwest)
            {
                return player.TryAction(Direction.SW);
            }
            else
            {
                return player.TryAction(Direction.NE);
            }
        }

        return false;
    }
}
