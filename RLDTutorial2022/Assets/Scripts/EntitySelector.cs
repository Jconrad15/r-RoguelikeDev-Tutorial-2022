using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntitySelector
{

    public static Entity SelectEntityPerLevel(
        Tile tile, int seed, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        // Use seed for this tile
        Random.InitState(seed);

        Entity newEntity;

        // Percentage that should be trolls
        float trollThreshold = 0.1825f * floorLevel;

        if (Random.value >= trollThreshold)
        {
            newEntity = Entity.SpawnCloneAtTile(
                EntityFactory.Instance.OrcPrefab, tile);
        }
        else
        {
            newEntity = Entity.SpawnCloneAtTile(
                EntityFactory.Instance.TrollPrefab, tile);
        }

        // Restore state
        Random.state = oldState;

        return newEntity;
    }

}
