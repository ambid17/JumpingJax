using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour {
    public bool isPaused;

    [SerializeField]
    GameObject inGameUI;

    [SerializeField]
    PauseMenu pauseMenu;

    [SerializeField]
    WinMenu winMenu;

    public Image crossHair;

    void Start () {
        inGameUI.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        winMenu.gameObject.SetActive(false);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.InitializePauseMenu();
        inGameUI.SetActive(false);
        winMenu.gameObject.SetActive(false);
    }

    public void UnPause()
    {
        Cursor.visible = false;
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        inGameUI.SetActive(true);
        winMenu.gameObject.SetActive(false);
    }

    public void ShowWinScreen()
    {
        Cursor.visible = true;
        winMenu.gameObject.SetActive(true);
        winMenu.levelText.text = "You beat level " + GameManager.GM.currentLevel.ToString();
        winMenu.completionTimeText.text = GetTimeString(GameManager.GM.completionTime);
    }

    string GetTimeString(float completionTime)
    {
        Debug.Log("showWin" + completionTime);
        TimeSpan time = TimeSpan.FromSeconds(completionTime);
        return time.ToString("hh':'mm':'ss");
    }
}
