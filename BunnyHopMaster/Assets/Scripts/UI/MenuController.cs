using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Transform levelButtonContainer;
    public GameObject levelObjectPrefab;

    public GameObject levelsPanel;
    public PauseMenu pauseMenu;

    public string[] sceneNames;

    private void Start()
    {
        levelsPanel.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        for(int i = 1; i <= PlayerStatsManager._PlayerStats.numberOfLevels; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonContainer);
            LevelButton levelButton = newLevelButton.GetComponentInChildren<LevelButton>();
            levelButton.SetupButton(i, PlayerStatsManager._PlayerStats.levels[i-1].isCompleted, sceneNames[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    public void ToggleSettingsMenu()
    {
        levelsPanel.SetActive(!levelsPanel.activeInHierarchy);
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeInHierarchy);
    }
}
