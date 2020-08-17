using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Transform levelButtonContainer;
    public GameObject levelObjectPrefab;
    public LevelPreview levelPreview;

    public GameObject homePanel;
    public GameObject levelSelectPanel;
    public GameObject achievementsPanel;
    public PauseMenu pauseMenu;

    private void Start()
    {
        Init();

        Level[] levels = GameManager.Instance.levelDataContainer.levels;
        for (int i = 0; i <= levels.Length - 1; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonContainer);
            LevelButton levelButton = newLevelButton.GetComponentInChildren<LevelButton>();
            levelButton.SetupButton(levels[i], () => OnClickLevel(levels[i]));
        }
    }

    
    public void Init()
    {
        homePanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        achievementsPanel.SetActive(false);
        pauseMenu.UnPause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelSelection()
    {
        homePanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        achievementsPanel.SetActive(false);
    }

    public void OnClickLevel(Level level)
    {
        //levelPreview.Init(level);
        GameManager.Instance.currentLevelBuildIndex = level.levelBuildIndex;
        SceneManager.LoadScene(level.levelBuildIndex);
    }

    public void Achievements()
    {
        homePanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        achievementsPanel.SetActive(true);
    }

    public void Options()
    {
        pauseMenu.Pause();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
