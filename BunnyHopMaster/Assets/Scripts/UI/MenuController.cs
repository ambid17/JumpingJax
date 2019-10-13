using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Transform levelButtonContainer;
    public GameObject levelObjectPrefab;

    private void Start()
    {
        for(int i = 1; i <= PlayerInfo.PI.numberOfLevels; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonContainer);
            LevelButton levelButton = newLevelButton.GetComponent<LevelButton>();
            levelButton.SetupButton(i, PlayerInfo.PI.levels[i-1].isCompleted);
        }
    }
}
