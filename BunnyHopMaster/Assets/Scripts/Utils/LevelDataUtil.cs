using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelDataUtil
{
    public static float GetTotalCompletionTime(LevelDataContainer container)
    {
        float totalTime = 0;
        foreach (Level level in container.levels)
        {
            totalTime = totalTime + level.completionTime;
        }
        return totalTime;
    }
}
