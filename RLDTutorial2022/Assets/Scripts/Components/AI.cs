using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : BaseComponent
{
    private Path_AStar pathway;

    public AI() : base()
    {
        pathway = null;
        Destination = null;
        NextTile = null;
    }

    public override object Clone()
    {
        return new AI();
    }

    public Tile Destination { get; private set; }
    public Tile NextTile { get; private set; }

    public bool TryAction(Entity aiEntity)
    {
        if (aiEntity == null)
        {
            Debug.LogError("No entity for this AI component");
            return false;
        }

        if (Destination != null)
        {
            Debug.Log("Destination is not null for " +
                aiEntity.EntityName + ", " +
                aiEntity.CurrentTile.Coordinates.ToString());
        }

        // If no path, reached end of path, or no destination
        // make a new destination and pathway
        if (pathway == null || 
            aiEntity.CurrentTile == Destination ||
            Destination == null)
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
        NextTile = pathway.Dequeue();
        //Debug.Log("Entity acted");
        return aiEntity.TryAction(NextTile);
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

        if (HexCoordinates.HexDistance(
            aiEntity.CurrentTile.Coordinates,
            player.CurrentTile.Coordinates) <=
            aiEntity.VisibilityDistance)
        {
            Destination = player.CurrentTile;
        }
        else
        {
            // Try to set destination as a random next door tile
            Tile randomNeighborTile = GameManager.Instance.Grid
                .GetTileInDirection(
                    aiEntity.CurrentTile,
                    Utility.GetRandomEnum<Direction>());

            if (randomNeighborTile.IsWalkable == false)
            {
                Destination = null;
                pathway = null;
                return;
            }
            Destination = randomNeighborTile;
        }

        // Create new pathway based on destination
        pathway = new Path_AStar(aiEntity.CurrentTile, Destination);
        // Trash first tile, this is the currentTile
        _ = pathway.Dequeue();
    }

}
