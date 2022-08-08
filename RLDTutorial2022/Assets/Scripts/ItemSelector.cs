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
        if (randomValue < 0.7f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.HealthPotionPrefab, tile);
        }
        else if (randomValue < 0.8f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.FireballScrollPrefab, tile);
        }
        else if (randomValue < 0.9f)
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.ConfusionScrollPrefab, tile);
        }
        else
        {
            newItem = Item.SpawnCloneAtTile(
                ItemFactory.Instance.LightingScrollPrefab, tile);
        }

        // Restore state
        Random.state = oldState;

        return newItem;
    }
}
