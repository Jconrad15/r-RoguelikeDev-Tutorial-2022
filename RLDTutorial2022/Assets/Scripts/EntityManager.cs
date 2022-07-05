using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<Entity> entities = new List<Entity>();

    private Action<Entity> cbOnPlayerCreated; 

    public void CreatePlayer(Tile startTile, Color playerColor)
    {
        Entity newPlayer =
            new Entity(startTile, "@", playerColor, true);

        entities.Add(newPlayer);
        cbOnPlayerCreated?.Invoke(newPlayer);
    }

    public void RegisterOnPlayerCreated(Action<Entity> callbackfunc)
    {
        cbOnPlayerCreated += callbackfunc;
    }

    public void UnregisterOnPlayerCreated(Action<Entity> callbackfunc)
    {
        cbOnPlayerCreated -= callbackfunc;
    }
}
