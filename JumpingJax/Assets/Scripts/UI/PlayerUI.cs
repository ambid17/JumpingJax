using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject inGameUI = null;

    [SerializeField]
    WinMenu winMenu = null;

    public Image crossHair;

    void Start () {
        inGameUI.SetActive(true);
        winMenu.gameObject.SetActive(false);
    }
	
	

    public void ShowWinScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inGameUI.SetActive(false);
        winMenu.gameObject.SetActive(true);

        winMenu.levelText.text = GameManager.GetCurrentLevel().levelName;
        winMenu.completionTimeText.text = "Completion time: " + GetTimeString(GameManager.Instance.currentCompletionTime);

        TimeSpan time = TimeSpan.FromSeconds(GameManager.GetCurrentLevel().completionTime);
        winMenu.bestTimeText.text = "Best time: " + time.ToString(PlayerConstants.levelCompletionTimeFormat);
    }

    string GetTimeString(float completionTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(completionTime);
        return time.ToString(PlayerConstants.levelCompletionTimeFormat);
    }
}
