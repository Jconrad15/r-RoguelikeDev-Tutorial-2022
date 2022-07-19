using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public bool IsPlayer { get; private set; }
    public string Character { get; private set; }
    public int VisibilityDistance { get; private set; } = 5;

    public string EntityName { get; private set; }

    private Action<Entity> cbOnEntityMoved;
    private Action<Entity, Direction> cbOnEntityAttack;

    public Tile CurrentTile { get; private set; }

    public Color Color { get; private set; }

    public bool BlocksMovement { get; private set; }

    public List<BaseComponent> Components { get; private set; }

    public Entity(string character, Color color,
        string entityName, List<BaseComponent> components, 
        int visibilityDistance = 5,
        bool isPlayer = false, bool blocksMovement = true)
    {
        IsPlayer = isPlayer;
        Character = character;
        EntityName = entityName;
        VisibilityDistance = visibilityDistance;
        Color = color;
        BlocksMovement = blocksMovement;

        Components = new List<BaseComponent>();
        for (int i = 0; i < components.Count; i++)
        {
            var component = components[i].Clone();
            if (component is AI)
            {
                Components.Add(component as AI);
            }
            else if (component is Fighter)
            {
                Components.Add(component as Fighter);
            }
        }

    }

    /// <summary>
    /// Copy clone Constructor.
    /// </summary>
    /// <param name="entityToClone"></param>
    /// <param name="targetTile"></param>
    private Entity(Entity entityToClone, Tile targetTile)
    {
        CurrentTile = targetTile;
        targetTile.entity = this;

        EntityName = entityToClone.EntityName;
        IsPlayer = entityToClone.IsPlayer;
        Character = entityToClone.Character;
        Color = entityToClone.Color;
        BlocksMovement = entityToClone.BlocksMovement;
        Components = entityToClone.Components;
    }

    public static Entity SpawnCloneAtTile(
        Entity entityPrefab, Tile tile)
    {
        return new Entity(entityPrefab, tile);
    }

    public bool TryAction(Direction direction)
    {
        Tile neighborTile = GameManager.Instance.Grid
            .GetTileInDirection(CurrentTile, direction);

        if (neighborTile == null) { return false; }
        if (neighborTile.IsWalkable == false) { return false; }
        
        // If entity exists and blocks movement, Attack instead
        if (neighborTile.entity != null)
        { 
            if (neighborTile.entity.BlocksMovement)
            {
                Attack(neighborTile, direction);
                return true;
            }
        }

        MoveTo(neighborTile);
        return true;
    }

    public bool TryAction(Tile targetTile)
    {
        if (targetTile == null) { return false; }
        if (targetTile.IsWalkable == false) { return false; }

        // If entity exists and blocks movement, Attack instead
        if (targetTile.entity != null)
        {
            if (targetTile.entity.BlocksMovement)
            {
                Attack(targetTile);
                return true;
            }
        }

        MoveTo(targetTile);
        return true;
    }

    private void Attack(Tile targetTile)
    {
        // TODO: this
        Debug.Log("Attack target tile");
    }

    private void Attack(Tile neighborTile, Direction direction)
    {
        Debug.Log("You kick the " +
            neighborTile.entity.EntityName);

        cbOnEntityAttack?.Invoke(this, direction);
    }

    private void MoveTo(Tile destination)
    {
        // Remove self from current tile
        CurrentTile.entity = null;

        CurrentTile = destination;
        CurrentTile.entity = this;

        cbOnEntityMoved?.Invoke(this);
    }

    public void RegisterOnEntityMoved(
        Action<Entity> callbackfunc)
    {
        cbOnEntityMoved += callbackfunc;
    }

    public void UnregisterOnEntityMoved(
        Action<Entity> callbackfunc)
    {
        cbOnEntityMoved -= callbackfunc;
    }

    public void RegisterOnEntityAttack(
        Action<Entity, Direction> callbackfunc)
    {
        cbOnEntityAttack += callbackfunc;
    }

    public void UnregisterOnEntityAttack(
        Action<Entity, Direction> callbackfunc)
    {
        cbOnEntityAttack -= callbackfunc;
    }
}
