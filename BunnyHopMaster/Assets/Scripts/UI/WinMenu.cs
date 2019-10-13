using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Text levelText;
    public Text completionTimeText;

    public void Retry()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(GameManager.GM.currentLevel);
    }

    public void NextLevel()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        SceneManager.LoadScene(GameManager.GM.currentLevel + 1);
    }

    
}
