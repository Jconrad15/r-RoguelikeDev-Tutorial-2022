using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceLogUI : MonoBehaviour
{
    public void Setup(string logText, Color color)
    {
        TextMeshProUGUI textObject =
            GetComponentInChildren<TextMeshProUGUI>();

        textObject.SetText(logText);
        textObject.color = color;
    }

}
