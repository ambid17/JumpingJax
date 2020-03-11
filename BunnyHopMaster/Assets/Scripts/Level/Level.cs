using UnityEngine;
using System;

[Serializable]
public class Level
{
    [Header("Set in Editor")]
    [SerializeField]
    public int levelBuildIndex;
    [SerializeField]
    public string levelName;
    [SerializeField]
    public bool isPortalLevel;
    [Header("Set in Game")]
    [SerializeField]
    public bool isCompleted;
    [SerializeField]
    public float completionTime;
    
}
