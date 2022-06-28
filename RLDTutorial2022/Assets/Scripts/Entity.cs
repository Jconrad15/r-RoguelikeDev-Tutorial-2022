using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public bool isPlayer = false;
    public string character;

    private Action<Entity> cbOnEntityMoved;

    public Tile CurrentTile { get; private set; }

    public Entity(
        Tile currentTile, string character,
        bool isPlayer = false)
    {
        CurrentTile = currentTile;
        this.isPlayer = isPlayer;
        this.character = character;

        // Set self to tile
        currentTile.entity = this;
    }

    public bool TryMove(Direction direction)
    {
        Tile neighborTile = GameManager.Instance.Grid
            .GetTileInDirection(CurrentTile, direction);

        if (neighborTile == null){ return false; }

        if (neighborTile.entity != null) { return false; }

        MoveTo(neighborTile);

        return true;
    }

    private void MoveTo(Tile destination)
    {
        // Remove self from current tile
        CurrentTile.entity = null;

        CurrentTile = destination;
        CurrentTile.entity = this;

        cbOnEntityMoved?.Invoke(this);

        Debug.Log("Move to: " + destination.coordinates.ToString());
    }

    public void RegisterOnEntityMoved(Action<Entity> callbackfunc)
    {
        cbOnEntityMoved += callbackfunc;
    }

    public void UnregisterOnEntityMoved(Action<Entity> callbackfunc)
    {
        cbOnEntityMoved -= callbackfunc;
    }
}
