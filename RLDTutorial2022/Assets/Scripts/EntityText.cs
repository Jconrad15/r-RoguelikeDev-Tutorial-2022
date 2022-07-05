using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityText : MonoBehaviour
{
    public void SetText(Entity entity)
    {
        TextMeshProUGUI text =
            GetComponentInChildren<TextMeshProUGUI>();

        text.SetText(entity.character);
        text.color = entity.Color;
    }


}
