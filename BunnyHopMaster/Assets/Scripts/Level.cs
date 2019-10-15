using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int levelNumber;
    public bool isCompleted;
    public string levelName;

    public Level(int levelNumber, int isCompleted, string levelName)
    {
        this.levelNumber = levelNumber;
        this.levelName = levelName;
        if(isCompleted == 0)
        {
            this.isCompleted = false;
        }
        else
        {
            this.isCompleted = true;
        }
    }
}
