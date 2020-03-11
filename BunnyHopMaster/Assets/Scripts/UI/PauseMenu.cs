using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    GameObject optionsMenu;

    [SerializeField]
    GameObject pauseMenuHome;

    private void OnEnable()
    {
        pauseMenuHome.SetActive(true);
        optionsMenu.SetActive(false);
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
