using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int levelNumber;
    public bool isCompleted;

    public Level(int levelNumber, int isCompleted)
    {
        this.levelNumber = levelNumber;
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
