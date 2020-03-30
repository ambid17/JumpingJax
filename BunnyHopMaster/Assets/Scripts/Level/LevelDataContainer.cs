using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/levelData")]
public class LevelDataContainer : ScriptableObject
{
    [SerializeField]
    public Level[] levels;
}