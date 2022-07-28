using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverEntity : MonoBehaviour
{
    private Entity entity;

    public void SetEntity(Entity entity) => this.entity = entity;

    private void OnMouseEnter()
    {
        InterfaceLogManager.Instance.LogMessage(
            "This is a " + entity.EntityName, entity.Color);
    }


}
