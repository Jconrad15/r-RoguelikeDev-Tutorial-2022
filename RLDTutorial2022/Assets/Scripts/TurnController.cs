using System;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { PlayerTurn, AITurn };
/// <summary>
/// Singleton directing turns
/// </summary>
public class TurnController : MonoBehaviour
{
    private TurnState state;

    private Action cbOnStartPlayerTurn;
    private Action cbOnStartAITurn;

    public static TurnController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartTurnSystem()
    {
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("Start player turn");
        cbOnStartPlayerTurn?.Invoke();
        state = TurnState.PlayerTurn;
    }

    private void StartAITurn()
    {
        // Enemy performs their turn
        // Raise event that states that it is the enemy's turn

        //Debug.Log("Raise AI turn event");

        state = TurnState.AITurn;
        cbOnStartAITurn?.Invoke();
    }

    public void NextTurn()
    {
        Debug.Log("Next turn is triggered");

        if (state == TurnState.PlayerTurn)
        {
            StartAITurn();
        }
        else if (state == TurnState.AITurn)
        {
            StartPlayerTurn();
        }
        else
        {
            Debug.LogError(
                "Something bad happened with the turn order.");
        }
    }

    public void RegisterOnStartPlayerTurn(Action callbackfunc)
    {
        cbOnStartPlayerTurn += callbackfunc;
    }

    public void UnregisterOnStartPlayerTurn(Action callbackfunc)
    {
        cbOnStartPlayerTurn -= callbackfunc;
    }

    public void RegisterOnStartAITurn(Action callbackfunc)
    {
        cbOnStartAITurn += callbackfunc;
    }

    public void UnregisterOnStartAITurn(Action callbackfunc)
    {
        cbOnStartAITurn -= callbackfunc;
    }
}