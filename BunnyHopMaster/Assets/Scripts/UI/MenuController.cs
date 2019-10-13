using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Transform levelButtonContainer;
    public GameObject levelObjectPrefab;

    private void Start()
    {
        for(int i = 1; i <= PlayerStatsManager._PlayerStats.numberOfLevels; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonContainer);
            LevelButton levelButton = newLevelButton.GetComponent<LevelButton>();
            levelButton.SetupButton(i, PlayerStatsManager._PlayerStats.levels[i-1].isCompleted);
        }
    }
}
