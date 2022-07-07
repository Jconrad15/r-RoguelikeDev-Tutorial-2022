using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EntityGOData
{
    public GameObject entityGO;
    public Entity entity;
    public LerpMovement entityMovement;

    public EntityGOData(
        GameObject entityGO, Entity entity,
        LerpMovement entityMovement)
    {
        this.entityGO = entityGO;
        this.entity = entity;
        this.entityMovement = entityMovement;
    }

    public bool ContainsEntity(Entity compareEntity)
    {
        return compareEntity == entity;
    }
}
