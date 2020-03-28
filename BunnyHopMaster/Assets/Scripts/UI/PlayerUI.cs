using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

<<<<<<< HEAD
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
        // Don't let the player pause the game if they are in the win menu
        // This would let the player unpause and play during the win menu
        if (GameManager.Instance.didWinCurrentLevel)
        {
            return;
        }

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
=======
public class PlayerUI : MonoBehaviour
    {
    public bool isPaused;

    [SerializeField]
    private GameObject inGameUI;

    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private WinMenu winMenu;

    public Image crossHair;

    private void Start()
        {
        inGameUI.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        winMenu.gameObject.SetActive(false);
        }

    private void Update()
        {
        // Don't let the player pause the game if they are in the win menu
        // This would let the player unpause and play during the win menu
        if (GameManager.Instance.didWinCurrentLevel)
            {
            return;
            }

        if (Input.GetKeyDown(KeyCode.Escape))
            {
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
>>>>>>> 5f7eea4... Initial commit
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        inGameUI.SetActive(false);
<<<<<<< HEAD
    }

    public void UnPause()
    {
=======
        Debug.Log("Pause");
        }

    public void UnPause()
        {
>>>>>>> 5f7eea4... Initial commit
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        inGameUI.SetActive(true);
<<<<<<< HEAD
    }

    public void ShowWinScreen()
    {
=======
        Debug.Log("Unpause");
        }

    public void ShowWinScreen()
        {
>>>>>>> 5f7eea4... Initial commit
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inGameUI.SetActive(false);
        winMenu.gameObject.SetActive(true);

        winMenu.levelText.text = GameManager.GetCurrentLevel().levelName;
        winMenu.completionTimeText.text = "Completion time: " + GetTimeString(GameManager.Instance.currentCompletionTime);

        TimeSpan time = TimeSpan.FromSeconds(GameManager.GetCurrentLevel().completionTime);
        winMenu.bestTimeText.text = "Best time: " + time.ToString(PlayerConstants.levelCompletionTimeFormat);
<<<<<<< HEAD
    }

    string GetTimeString(float completionTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(completionTime);
        return time.ToString(PlayerConstants.levelCompletionTimeFormat);
    }
}
=======
        }

    private string GetTimeString(float completionTime)
        {
        TimeSpan time = TimeSpan.FromSeconds(completionTime);
        return time.ToString(PlayerConstants.levelCompletionTimeFormat);
        }
    }
>>>>>>> 5f7eea4... Initial commit
