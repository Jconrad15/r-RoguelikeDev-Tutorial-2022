using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI characterText;

    public void SetText(Tile t)
    {
        Color selectedTextColor;
        if (t.VisibilityLevel ==
            VisibilityLevel.NotVisible)
        {
            selectedTextColor = new Color32(0, 0, 0, 0);
        }
        else if (t.VisibilityLevel ==
            VisibilityLevel.PreviouslySeen)
        {
            selectedTextColor = t.foregroundColor;
        }
        else // Visible
        {
            selectedTextColor = t.foregroundColor;
        }

        characterText.SetText(t.Character.ToString());
        characterText.color = selectedTextColor;
    }


}
