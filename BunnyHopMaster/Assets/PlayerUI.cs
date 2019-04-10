using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Button quitButton;

    void Start() {
        quitButton.onClick.AddListener(delegate { OnQuitPressed(); });
    }

    void OnQuitPressed() {
        Application.Quit();
    }

    void Update() {

    }
}
