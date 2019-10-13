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
        SceneManager.LoadScene(GameManager._GameManager.currentLevel);
    }

    public void NextLevel()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(GameManager._GameManager.currentLevel + 1);
    }

    public void GoToMainMenu()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
