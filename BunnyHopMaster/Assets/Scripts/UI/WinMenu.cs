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
        SceneManager.LoadScene(GameManager._GameManager.currentLevel);
    }

    public void NextLevel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(GameManager._GameManager.currentLevel == PlayerStatsManager._PlayerStats.levels.Length)
        {
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
        else
        {
            Cursor.visible = false;
            SceneManager.LoadScene(GameManager._GameManager.currentLevel + 1);
        }
    }

    public void GoToMainMenu()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
