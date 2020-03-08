using UnityEngine;
using System;

[Serializable]
public class Level
{
    [SerializeField]
    public int levelBuildIndex;
    [SerializeField]
    public string levelName;

    [SerializeField]
    public bool isCompleted;
    [SerializeField]
    public float completionTime;

    //public Level(int levelNumber, float completionTime, string levelName)
    //{
    //    this.levelBuildIndex = levelNumber;
    //    this.levelName = levelName;
    //    if(completionTime == float.PositiveInfinity)
    //    {
    //        this.isCompleted = false;
    //    }
    //    else
    //    {
    //        this.isCompleted = true;
    //    }
    //    this.completionTime = completionTime;
    //}
}
