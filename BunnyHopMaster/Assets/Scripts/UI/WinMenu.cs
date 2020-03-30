using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Text levelText;
    public Text completionTimeText;
    public Text bestTimeText;

    public void Retry()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(GameManager.Instance.currentLevelBuildIndex);
    }

    public void NextLevel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        //This assumes the main menu is build index 0
        if(GameManager.Instance.currentLevelBuildIndex >= GameManager.Instance.levelDataContainer.levels.Length)
        {
            Cursor.visible = true;
            // Load credits scene
            SceneManager.LoadScene(GameManager.Instance.currentLevelBuildIndex + 1);
        }
        else
        {
            Cursor.visible = false;
            GameManager.Instance.currentLevelBuildIndex++;
            SceneManager.LoadScene(GameManager.Instance.currentLevelBuildIndex);
        }
    }

    public void GoToMainMenu()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerConstants.BuildSceneIndex);
    }
}
