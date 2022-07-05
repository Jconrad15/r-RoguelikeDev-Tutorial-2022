using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    [SerializeField]
    private Color playerColor;

    public void StartButton()
    {
        GameManager.Instance.GameStart(playerColor);
    }

}
