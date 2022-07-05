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
        characterText.SetText(t.Character.ToString());
    }


}
