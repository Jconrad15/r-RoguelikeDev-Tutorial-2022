using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AI : BaseComponent
{
    [NonSerialized]
    private Path_AStar pathway;

    private int isConfusedForTurns = 0;

    public AI() : base()
    {
        NullDataValues();
    }

    public override object Clone()
    {
        return new AI();
    }

    [NonSerialized]
    private Tile destination;
    [NonSerialized]
    private Tile nextTile;

    public bool TryAction(Entity aiEntity, int attemptCount)
    {
        if (aiEntity == null)
        {
            Debug.LogError("No entity for this AI component");
            return false;
        }

        // Reset variables for each attempt
        NullDataValues();

        // Check if there is a destination
        // and if the player is still there
        bool playerAtDestination = false;
        if (destination != null)
        {
            playerAtDestination = GameManager.Instance
                .EntityManager.GetPlayerEntity()
                .CurrentTile != destination;
        }

        // If no path, reached end of path, or no destination,
        // or if player no longer at destination
        // make a new destination and pathway
        if (pathway == null || 
            aiEntity.CurrentTile == destination ||
            destination == null ||
            playerAtDestination == false)
        {
            DetermineNewDestinationPath(aiEntity);
            // Exit if pathway is still null
            if (pathway == null) { return false; }
        }

        // Try action at next pathway tile
        if (pathway.Length() <= 0)
        {
            pathway = null;
            return false;
        }
        
        // First peek at next tile. 
        nextTile = pathway.Peek();

        float distance = HexCoordinates.HexDistance(
            nextTile.Coordinates,
            aiEntity.CurrentTile.Coordinates);
        if (distance > 1)
        {
            Debug.LogError("Next-current tile distance > 1.");
        }

        bool acted = aiEntity.TryAction(nextTile);
        // If entity acts, then tile will be dequeued
        if (acted)
        {
            _ = pathway.Dequeue();
        }

        return acted;
    }

    private void DetermineNewDestinationPath(Entity aiEntity)
    {
        // Determine new destination
        Entity player = GameManager.Instance
            .EntityManager.GetPlayerEntity();

        if (player == null)
        {
            Debug.LogError("No player");
            pathway = null;
            return;
        }
        bool isVisibilityDistToPlayer = HexCoordinates.HexDistance(
            aiEntity.CurrentTile.Coordinates,
            player.CurrentTile.Coordinates) <=
            aiEntity.VisibilityDistance;

        if (isVisibilityDistToPlayer && isConfusedForTurns <= 0)
        {
            destination = player.CurrentTile;
        }
        else
        {
            // Try to set destination as a random next door tile
            Tile randomNeighborTile = GameManager.Instance.CurrentGrid
                .GetTileInDirection(
                    aiEntity.CurrentTile,
                    Utility.GetRandomEnum<Direction>());

            if (randomNeighborTile == null)
            {
                NullDataValues();
                return;
            }

            if (randomNeighborTile.IsWalkable == false)
            {
                NullDataValues();
                return;
            }
            destination = randomNeighborTile;
        }

        // Create new pathway based on destination
        pathway = new Path_AStar(aiEntity.CurrentTile, destination);
        // Trash first tile, this is the currentTile
        _ = pathway.Dequeue();

        float distance = HexCoordinates.HexDistance(
            pathway.Peek().Coordinates,
            aiEntity.CurrentTile.Coordinates);
        if (distance > 1)
        {
            Debug.LogError("Next-current tile distance > 1.");
        }

        TryDecreaseConfusion();
    }

    private void TryDecreaseConfusion()
    {
        if (isConfusedForTurns > 0)
        {
            isConfusedForTurns -= 1;

            if (isConfusedForTurns == 0)
            {
                InterfaceLogManager.Instance.LogMessage(
                    e.EntityName + " is no longer confused.");
            }
        }

    }

    public void ConfuseAI(int turns)
    {
        isConfusedForTurns = turns;
    }

    private void NullDataValues()
    {
        destination = null;
        pathway = null;
        nextTile = null;
    }

}
