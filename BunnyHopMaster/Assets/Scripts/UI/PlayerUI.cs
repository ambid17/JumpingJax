using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public bool isPaused;

    [SerializeField]
    GameObject inGameUI;

    [SerializeField]
    GameObject pauseMenu;

    public Image crossHair;

    void Start () {
        pauseMenu.SetActive(false);
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
        pauseMenu.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void UnPause()
    {
        Cursor.visible = false;
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        inGameUI.SetActive(true);
    }
}
