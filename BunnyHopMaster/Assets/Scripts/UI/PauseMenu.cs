using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    GameObject optionsMenu;

    [SerializeField]
    GameObject pauseMenuHome;

    public void ToggleOptionsMenu() {
        pauseMenuHome.SetActive(!pauseMenuHome.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
