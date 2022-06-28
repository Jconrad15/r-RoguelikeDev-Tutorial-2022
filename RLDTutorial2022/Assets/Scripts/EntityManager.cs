using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private List<Entity> entities = new List<Entity>();

    private Action<Entity> cbOnPlayerCreated; 

    public void CreatePlayer(Tile startTile)
    {
        Entity newPlayer = new Entity(startTile, "@", true);

        entities.Add(newPlayer);
        cbOnPlayerCreated?.Invoke(newPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
