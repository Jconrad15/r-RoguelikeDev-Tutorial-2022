using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public bool isPlayer = false;
    public string character;
    public int visibilityDistance = 5;

    private Action<Entity> cbOnEntityMoved;

    public Tile CurrentTile { get; private set; }

    public Color Color { get; private set; }

    public bool BlocksMovement { get; private set; }

    public Entity(string character, Color color,
        bool isPlayer = false, bool blocksMovement = true)
    {
        this.isPlayer = isPlayer;
        this.character = character;

        Color = color;
        BlocksMovement = blocksMovement;
    }

/*    public Entity(
        Tile currentTile, string character,
        Color color, bool isPlayer = false)
    {
        CurrentTile = currentTile;
        this.isPlayer = isPlayer;
        this.character = character;
        Color = color;

        BlocksMovement = true;

        // Set self to tile
        currentTile.entity = this;
    }*/

    private Entity(Entity entityToClone, Tile targetTile)
    {
        CurrentTile = targetTile;
        targetTile.entity = this;

        isPlayer = entityToClone.isPlayer;
        character = entityToClone.character;
        Color = entityToClone.Color;
        BlocksMovement = entityToClone.BlocksMovement;
    }

    public static Entity SpawnCloneAtTile(
        Entity entityPrefab, Tile tile)
    {
        return new Entity(entityPrefab, tile);
    }

    public bool TryMove(Direction direction)
    {
        Tile neighborTile = GameManager.Instance.Grid
            .GetTileInDirection(CurrentTile, direction);

        if (neighborTile == null) { return false; }
        if (neighborTile.IsWalkable == false) { return false; }
        
        // if entity exists and blocks movement, dont move
        if (neighborTile.entity != null) 
        { 
            if (neighborTile.entity.BlocksMovement)
            {
                return false;
            }
        }

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
