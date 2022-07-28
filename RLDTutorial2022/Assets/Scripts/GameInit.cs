using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public void StartButton()
    {
        EntityFactory.Instance.InitializeFactory();
        ItemFactory.Instance.InitializeFactory();
        GameManager.Instance.GameStart();
    }

}
