using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectText : MonoBehaviour
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
            selectedTextColor = ColorDatabase.hidden;
        }
        else // Visible
        {
            selectedTextColor = entity.Color;
        }

        text.SetText(entity.Character);
        text.color = selectedTextColor;
    }

    public void SetText(Item item)
    {
        TextMeshProUGUI text =
            GetComponentInChildren<TextMeshProUGUI>();

        Tile t = item.CurrentTile;

        Color selectedTextColor;
        if (t.VisibilityLevel == VisibilityLevel.NotVisible ||
            t.VisibilityLevel == VisibilityLevel.PreviouslySeen)
        {
            selectedTextColor = ColorDatabase.hidden;
        }
        else // Visible
        {
            selectedTextColor = item.Color;
        }

        text.SetText(item.Character);
        text.color = selectedTextColor;
    }

}
