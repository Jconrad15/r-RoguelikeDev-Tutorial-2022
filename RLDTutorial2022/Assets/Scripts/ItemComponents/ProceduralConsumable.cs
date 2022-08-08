using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProceduralItemTargetingType
{
    NearestEntity, SpecificTarget
};

[SerializeField]
public class ProceduralConsumable : Consumable
{
    [JsonProperty]
    public int DamageAmount { get; protected set; }
    [JsonProperty]
    public int Radius { get; protected set; }
    [JsonProperty]
    public int MaximumRange { get; protected set; }

    [JsonProperty]
    public string Character { get; protected set; }
    [JsonProperty]
    public Color ItemColor { get; protected set; }
    [JsonProperty]
    public string ItemName { get; protected set; }
    [JsonProperty]
    private string itemTerm;

    [JsonProperty]
    public ProceduralItemTargetingType TargetingType
    { get; protected set; }

    [JsonConstructor]
    public ProceduralConsumable() : base()
    {
        GenerateNewProceduralConsumable();
    }

    private void GenerateNewProceduralConsumable()
    {
        DamageAmount = Random.Range(3, 15);

        Radius = Random.Range(0, 2);
        MaximumRange = Random.Range(5, 7);

        Character = "?";

        CreateType();

        TargetingType = Utility
            .GetRandomEnum<ProceduralItemTargetingType>();
    }

    private void CreateType()
    {
        ProceduralTypes generatedType = GetRandomType();
        ItemColor = generatedType.color;
        ItemName = generatedType.name;
        itemTerm = generatedType.term;
    }

    private struct ProceduralTypes
    {
        public Color color;
        public string name;
        public string term;
    }

    private static ProceduralTypes GetRandomType()
    {
        List<ProceduralTypes> types = new List<ProceduralTypes>()
        {
            new ProceduralTypes
            {
                color = ColorDatabase.water,
                name = "Mystery Water Scroll",
                term = "water"
            },

            new ProceduralTypes
            {
                color = ColorDatabase.fire,
                name = "Mystery Fire Scroll",
                term = "fire"
            },

            new ProceduralTypes
            {
                color = ColorDatabase.yellow,
                name = "Mystery lighting Scroll",
                term = "lighting"
            },

            new ProceduralTypes
            {
                color = ColorDatabase.earth,
                name = "Mystery earth Scroll",
                term = "earth"
            },

            new ProceduralTypes
            {
                color = ColorDatabase.air,
                name = "Mystery air Scroll",
                term = "air"
            },

            new ProceduralTypes
            {
                color = ColorDatabase.corruption,
                name = "Mystery corruption Scroll",
                term = "corruption"
            },
        };

        ProceduralTypes type = types[Random.Range(0, types.Count)];
        return type;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="other"></param>
    public ProceduralConsumable(ProceduralConsumable other)
        : base(other)
    {
        DamageAmount = other.DamageAmount;
        Radius = other.Radius;
        MaximumRange = other.MaximumRange;
        Character = other.Character;
        ItemColor = other.ItemColor;
        ItemName = other.ItemName;
        itemTerm = other.itemTerm;
        TargetingType = other.TargetingType;
    }

    public override object Clone()
    {
        return new ProceduralConsumable(this);
    }

    public override bool Activate(Entity entity)
    {
        if (TargetingType == ProceduralItemTargetingType
            .SpecificTarget)
        {
            return TryActivateSpecificTarget(entity);
        }
        else if (TargetingType == ProceduralItemTargetingType
            .NearestEntity)
        {
            return TryActivateNearestTarget(entity);
        }

        return false;
    }

    private bool TryActivateNearestTarget(Entity entity)
    {
        Fighter f = entity.TryGetFighterComponent();
        if (f == null)
        {
            Debug.LogError(
                "Attacker does not have fighter component");
            return false;
        }

        // Find the nearest other entity
        Entity otherEntity = GameManager.Instance
            .EntityManager.GetNearestFighterEntity(entity);

        // Don't use if not in range
        int distance = HexCoordinates.HexDistance(
            entity.CurrentTile.Coordinates,
            otherEntity.CurrentTile.Coordinates);
        if (distance > MaximumRange)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No target in range.",
                ItemColor);
            return false;
        }

        // Get other fighter component
        Fighter otherFighter = otherEntity.TryGetFighterComponent();
        if (otherFighter == null)
        {
            Debug.LogError(
                "Attacked entity does not have fighter component");
            return false;
        }

        // Damage other entity's fighter component
        otherFighter.Damage(DamageAmount, entity);

        InterfaceLogManager.Instance.LogMessage(
            GetDamageDisplayText(otherEntity),
            ItemColor);
        return true;
    }

    private string GetDamageDisplayText(Entity otherEntity)
    {
        return otherEntity.EntityName +
                    " is hit with " +
                    DamageAmount +
                    " " +
                    itemTerm +
                    " damage!";
    }

    private bool TryActivateSpecificTarget(Entity entity)
    {
        Fighter f = entity.TryGetFighterComponent();
        if (f == null)
        {
            Debug.LogError(
                "Attacker does not have fighter component");
            return false;
        }

        // Start targeting system
        TargetingSystem.Instance.TryGetTarget(this, false, Radius);
        // Return false, since this is not yet used
        return false;
    }

    /// <summary>
    /// Called when tile is targeted by player input,
    /// after activating the item.
    /// </summary>
    /// <param name="targetTile"></param>
    public override void TileTargeted(Tile targetTile)
    {
        if (targetTile == null)
        {
            InterfaceLogManager.Instance.LogMessage(
                "No tile targeted",
                ColorDatabase.error);
            return;
        }
        // Damage at the target tile
        TryDamageEntityAtTile(targetTile);
        // Damage surrounding tiles if radius is 1
        if (Radius == 1)
        {
            // Damage at neighboring tiles
            Tile[] neighbors = targetTile.GetNeighboringTiles();
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null)
                {
                    TryDamageEntityAtTile(neighbors[i]);
                }
            }
        }
        // If this item component is used, then notify item
        item.ItemUsedThroughTargeting();
    }

    private void TryDamageEntityAtTile(Tile tile)
    {
        Entity e = tile.entity;
        if (e != null)
        {
            Fighter f = e.TryGetFighterComponent();
            if (f != null)
            {
                InterfaceLogManager.Instance.LogMessage(
                    GetDamageDisplayText(e),
                    ItemColor);
                f.Damage(DamageAmount, e);
            }
        }
    }
}
