using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Entity
{
    public bool IsPlayer { get; private set; }
    public string Character { get; private set; }
    public int VisibilityDistance { get; private set; } = 5;

    public string EntityName { get; private set; }

    private Action<Entity> cbOnEntityMoved;
    private Action<Entity, Direction> cbOnEntityAttackDirection;
    private Action<Entity> cbOnEntityDied;

    [JsonIgnore]
    public Tile CurrentTile { get; private set; }
    public void SetTile(Tile t) => CurrentTile = t;

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

        CloneComponents(components);
    }

    private void CloneComponents(List<BaseComponent> components)
    {
        Components = new List<BaseComponent>();
        for (int i = 0; i < components.Count; i++)
        {
            // Clone the prefab's component
            var component = components[i].Clone();
            if (component is AI)
            {
                AI ai = component as AI;
                Components.Add(ai);
                ai.SetEntity(this);
            }
            else if (component is Fighter)
            {
                Fighter f = component as Fighter;
                Components.Add(f);
                f.SetEntity(this);
            }
            else if (component is Inventory)
            {
                Inventory inv = component as Inventory;
                Components.Add(inv);
                inv.SetEntity(this);
            }
            else
            {
                Debug.LogError(
                    "Entity BaseComponent not cast as anything.");
            }
        }
    }

    /// <summary>
    /// Constructor that creates entities from loaded data.
    /// </summary>
    /// <param name="savedEntity"></param>
    private Entity(SavedEntity savedEntity, Tile targetTile)
    {
        IsPlayer = savedEntity.isPlayer;
        Character = savedEntity.character;
        EntityName = savedEntity.entityName;
        VisibilityDistance = savedEntity.visibilityDistance;
        Color = SavedColor.LoadColor(savedEntity.color);
        BlocksMovement = savedEntity.blocksMovement;
        CurrentTile = targetTile;
        Components = savedEntity.components.ToList();
    }

    /// <summary>
    /// Copy clone Constructor.
    /// </summary>
    /// <param name="entityToClone"></param>
    /// <param name="targetTile"></param>
    private static Entity EntityClone(
        Entity entityToClone, Tile targetTile)
    {
        Entity e = new Entity(
            entityToClone.Character,
            entityToClone.Color,
            entityToClone.EntityName,
            entityToClone.Components,
            entityToClone.VisibilityDistance,
            entityToClone.IsPlayer,
            entityToClone.BlocksMovement);

        e.CurrentTile = targetTile;
        targetTile.entity = e;

        return e;
    }

    private static Entity EntityClone(
        SavedEntity savedEntityToClone, Tile targetTile)
    {
        Entity e = new Entity(
            savedEntityToClone, targetTile);

        if (e.Components != null)
        {
            // Set entity in components 
            for (int i = 0; i < e.Components.Count; i++)
            {
                if (e.Components[i] == null)
                {
                    continue;
                }

                e.Components[i].SetEntity(e);
            }
        }

        e.CurrentTile = targetTile;
        targetTile.entity = e;

        return e;
    }

    public static Entity SpawnCloneAtTile(
        SavedEntity savedEntity, Tile tile)
    {
        return EntityClone(savedEntity, tile);
    }

    public static Entity SpawnCloneAtTile(
        Entity entityPrefab, Tile tile)
    {
        return EntityClone(entityPrefab, tile);
    }

    public bool TryPickUpItem()
    {
        if (CurrentTile.item == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No item to pick up.");
            return false;
        }

        Inventory inventory = TryGetInventoryComponent();
        if (inventory == null)
        {
            Debug.LogError("No inventory");
        }

        return inventory.TryAddItem(CurrentTile.item);
    }

    public bool TryGoDownStairs()
    {
        // Check if on stairs
        if (CurrentTile.Character != '>') { return false; }


        GameManager.Instance.GoDownStairs();
        return true;
    }

    public bool TryAction(Direction direction)
    {
        Tile neighborTile = GameManager.Instance.CurrentGrid
            .GetTileInDirection(CurrentTile, direction);

        if (neighborTile == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "The way is blocked.");
            return false;
        }
        if (neighborTile.IsWalkable == false)
        {
            InterfaceLogManager.Instance.LogMessage(
                "The way is blocked.");
            return false;
        }
        
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
            if (targetTile.entity.IsPlayer)
            {
                Attack(targetTile);
                return true;
            }
            else
            {
                return false;
            }

        }

        MoveTo(targetTile);
        return true;
    }

    private void Attack(Tile targetTile)
    {
        Fighter fighter = TryGetFighterComponent();
        if (fighter == null) { return; }

        Entity targetEntity = targetTile.entity;
        if (targetEntity == null) { return; }

        Fighter target = targetEntity.TryGetFighterComponent();
        if (target == null) { return; }

        int damage = fighter.power - target.defense;

        if (damage > 0)
        {
            InterfaceLogManager.Instance.LogMessage(EntityName +
                " did " + damage + " damage to " +
                targetEntity.EntityName);
            // Do the damage
            target.Damage(damage);
        }
        else
        {
            InterfaceLogManager.Instance.LogMessage(
                EntityName + " attacked " +
                targetEntity.EntityName + "for no damage");
        }

    }

    public Inventory TryGetInventoryComponent()
    {
        Inventory inventory = null;
        for (int i = 0; i < Components.Count; i++)
        {
            var component = Components[i];
            if (component is Inventory)
            {
                inventory = component as Inventory;
            }
        }

        return inventory;
    }

    public Fighter TryGetFighterComponent()
    {
        Fighter fighter = null;
        for (int i = 0; i < Components.Count; i++)
        {
            var component = Components[i];
            if (component is Fighter)
            {
                fighter = component as Fighter;
            }
        }

        return fighter;
    }

    public AI TryGetAIComponent()
    {
        AI ai = null;
        for (int i = 0; i < Components.Count; i++)
        {
            var component = Components[i];
            if (component is AI)
            {
                ai = component as AI;
            }
        }

        return ai;
    }

    private void Attack(Tile neighborTile, Direction direction)
    {
        cbOnEntityAttackDirection?.Invoke(this, direction);
        Attack(neighborTile);
    }

    private void MoveTo(Tile destination)
    {
        // Remove self from current tile
        CurrentTile.entity = null;

        CurrentTile = destination;
        CurrentTile.entity = this;

        cbOnEntityMoved?.Invoke(this);
    }

    public void Died()
    {
        if (!IsPlayer)
        {
            InterfaceLogManager.Instance.LogMessage(
                EntityName + " is dead",
                ColorDatabase.death);
        }

        // Edit entity
        Character = "%";
        Color = ColorDatabase.death;
        BlocksMovement = false;
        EntityName = "Remains of " + EntityName;

        // Remove components
        for (int i = 0; i < Components.Count; i++)
        {
            Components[i].Destroy();
            Components[i] = null;
        }
        Components = new List<BaseComponent>();

        cbOnEntityDied?.Invoke(this);
    }

    public void Destroy()
    {
        Character = null;
        EntityName = null;
        Components = null;

        ClearCallbacks();
    }

    public void ClearCallbacks()
    {
        cbOnEntityAttackDirection = null;
        cbOnEntityDied = null;
        cbOnEntityMoved = null;
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

    public void RegisterOnEntityAttackDirection(
        Action<Entity, Direction> callbackfunc)
    {
        cbOnEntityAttackDirection += callbackfunc;
    }

    public void UnregisterOnEntityAttackDirection(
        Action<Entity, Direction> callbackfunc)
    {
        cbOnEntityAttackDirection -= callbackfunc;
    }

    public void RegisterOnEntityDied(
    Action<Entity> callbackfunc)
    {
        cbOnEntityDied += callbackfunc;
    }

    public void UnregisterOnEntityDied(
        Action<Entity> callbackfunc)
    {
        cbOnEntityDied -= callbackfunc;
    }

}
