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
    private KeyCode actionKey = KeyCode.Space;

    private bool isModal;

    private Entity player;
    private EscapeMenuManager escManager;

    private void Start()
    {
        FindObjectOfType<EntityManager>()
            .RegisterOnPlayerCreated(OnPlayerCreated);

        TurnController.Instance
            .RegisterOnStartPlayerTurn(OnStartTurn);

        escManager = FindObjectOfType<EscapeMenuManager>();
    }

    private void OnPlayerCreated(Entity player)
    {
        this.player = player;
        isModal = false;
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

        _ = StartCoroutine(PlayerProcessing());
    }

    private IEnumerator PlayerProcessing()
    {
        // Leave loop and end turn when player moves
        bool playerActed = false;
        while (playerActed == false)
        {
            // Can act if not modal (targeting, level up, )
            // and escape menu is closed
            if (isModal == false &&
                escManager.IsOpen == false)
            {
                playerActed = CheckDirectionalAction();
            }

            yield return null;
        }

        // Turn is done
        TurnController.Instance.NextTurn();
    }

    private bool CheckDirectionalAction()
    {
        if (Input.GetKeyDown(actionKey))
        {
            // Actions

            // Try pick up item
            bool itemPickedUp = player.TryPickUpItem();
            if (itemPickedUp == true) { return itemPickedUp; }

            // Try use down stairs
            bool goDownStairs = player.TryGoDownStairs();
            if (goDownStairs == false) { return goDownStairs; }

            return false;
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

    public void StartModal()
    {
        isModal = true;
    }

    public void StopModal()
    {
        isModal = false;
    }
}
