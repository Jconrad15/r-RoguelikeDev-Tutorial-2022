using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceLogUI : MonoBehaviour
{
    public void Setup(string logText)
    {
        TextMeshProUGUI textObject =
            GetComponentInChildren<TextMeshProUGUI>();

        textObject.SetText(logText);
    }

}
