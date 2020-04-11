using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Level X", menuName = "ScriptableObjects/level")]
public class Level : ScriptableObject
{
    [Header("Set in Editor")]
    [SerializeField]
    public int levelBuildIndex;
    [SerializeField]
    public string levelName;
    [SerializeField]
    public bool isPortalLevel;
    [SerializeField]
    public bool isSurfLevel;
    [SerializeField]
    public string[] tutorialTexts;
    [SerializeField]
    public int numberOfCheckpoints;
    [Header("Set in Game")]
    [SerializeField]
    public bool isCompleted;
    [SerializeField]
    public float completionTime;
}