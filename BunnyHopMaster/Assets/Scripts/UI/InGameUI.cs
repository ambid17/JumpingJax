using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Text completionTimeText;

    void Update()
    {
        if(GameManager._GameManager != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(GameManager._GameManager.completionTime);
            completionTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
        }
    }
}
