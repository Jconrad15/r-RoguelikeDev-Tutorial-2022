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

        Tile t = entity.CurrentTile;

        Color selectedTextColor;
        if (t.VisibilityLevel == VisibilityLevel.NotVisible ||
            t.VisibilityLevel == VisibilityLevel.PreviouslySeen)
        {
            selectedTextColor = new Color32(0, 0, 0, 0);
        }
        else // Visible
        {
            selectedTextColor = entity.Color;
        }

        text.SetText(entity.Character);
        text.color = selectedTextColor;
    }


}
