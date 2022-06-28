using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{

    public void StartButton()
    {
        GameManager.Instance.GameStart();
    }

}
