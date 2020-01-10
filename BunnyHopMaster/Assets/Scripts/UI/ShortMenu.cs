using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortMenu : MonoBehaviour {
	
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
