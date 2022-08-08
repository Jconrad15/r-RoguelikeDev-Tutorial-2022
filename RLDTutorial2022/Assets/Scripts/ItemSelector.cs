using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector
{
    public static Item SelectItemPerLevel(
        Tile tile, int seed, int floorLevel)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        // Use seed for this tile
        Random.InitState(seed);

        Item newItem;
        float randomValue = Random.value;
        if (randomValue < 0.4f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.HealthPotionPrefab, tile);
        }
        else if (randomValue < 0.5f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.FireballScrollPrefab, tile);
        }
        else if (randomValue < 0.6f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.ConfusionScrollPrefab, tile);
        }
        else if (randomValue < 0.7f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.LightingScrollPrefab, tile);
        }
        else
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.CreateProceduralScroll(), tile);
        }

        // Restore state
        Random.state = oldState;

        return newItem;
    }
}
