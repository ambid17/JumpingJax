using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public Transform levelButtonContainer;
    public GameObject levelObjectPrefab;

    public GameObject levelsPanel;
    public PauseMenu pauseMenu;

    private void Start()
    {
        levelsPanel.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        Level[] levels = GameManager.Instance.levelDataContainer.levels;

        for (int i = 0; i <= levels.Length - 1; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonContainer);
            LevelButton levelButton = newLevelButton.GetComponentInChildren<LevelButton>();
            levelButton.SetupButton(levels[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.PauseMenu))
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
