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
        TimeSpan time = TimeSpan.FromSeconds(GameManager.GM.completionTime);
        completionTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
    }
}
