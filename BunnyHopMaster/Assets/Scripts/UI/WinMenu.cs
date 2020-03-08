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
        SceneManager.LoadScene(GameManager.Instance.currentLevelIndex);
    }

    public void NextLevel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(GameManager.Instance.currentLevelIndex == GameManager.Instance.levelDataContainer.levels.Length - 1)
        {
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
        else
        {
            Cursor.visible = false;
            GameManager.Instance.currentLevelIndex++;
            SceneManager.LoadScene(GameManager.Instance.currentLevelIndex);
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
