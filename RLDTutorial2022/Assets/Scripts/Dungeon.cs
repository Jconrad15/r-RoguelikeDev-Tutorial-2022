using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Dungeon
{
    public int[] dungeonGridSeeds;

    public Dungeon(int[] dungeonGridSeeds)
    {
        this.dungeonGridSeeds = dungeonGridSeeds;
    }

    public static Dungeon RandomDungeon()
    {
        int[] randomSeeds = new int[5];
        for (int i = 0; i < randomSeeds.Length; i++)
        {
            randomSeeds[i] = Random.Range(-10000, 10000);
        }

        return new Dungeon(randomSeeds);
    }
}
