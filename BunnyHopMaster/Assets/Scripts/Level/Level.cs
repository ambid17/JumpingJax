using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    [SerializeField]
    public int levelNumber;
    [SerializeField]
    public bool isCompleted;
    [SerializeField]
    public string levelName;
    [SerializeField]
    public float completionTime;

    public Level(int levelNumber, float completionTime, string levelName)
    {
        this.levelNumber = levelNumber;
        this.levelName = levelName;
        if(completionTime == float.PositiveInfinity)
        {
            this.isCompleted = false;
        }
        else
        {
            this.isCompleted = true;
        }
        this.completionTime = completionTime;
    }
}
