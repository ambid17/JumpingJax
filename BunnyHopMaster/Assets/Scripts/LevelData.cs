using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData LD;

    public int numberOfCheckpoints = 0;

    void Awake()
    {
        if (LevelData.LD == null)
        {
            LevelData.LD = this;
        }
        else if (LevelData.LD == this)
        {
            Destroy(LevelData.LD.gameObject);
            LevelData.LD = this;
        }
    }
}
