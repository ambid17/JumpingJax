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
}
