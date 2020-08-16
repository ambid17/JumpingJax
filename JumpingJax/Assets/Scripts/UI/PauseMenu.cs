using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    GameObject optionsMenu = null;

    [SerializeField]
    GameObject pauseMenuHome = null;

    [SerializeField]
    GameObject pauseMenuContainer = null;

    private  bool isPaused;


    private void Start()
    {
        pauseMenuContainer.SetActive(false);
    }

    void Update()
    {
        // Don't let the player pause the game if they are in the win menu
        // This would let the player unpause and play during the win menu
        if (GameManager.Instance.didWinCurrentLevel)
        {
            return;
        }

        if (Input.GetKeyDown(PlayerConstants.PauseMenu))
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
        Time.timeScale = 0;
        pauseMenuContainer.SetActive(true);
        pauseMenuHome.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void UnPause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuContainer.SetActive(false);
    }

    public void ToggleOptionsMenu() {
        pauseMenuHome.SetActive(!pauseMenuHome.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
