using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelButton : MonoBehaviour
{
    public Text buttonText;

    public void SetupButton(int level, bool isLevelCompleted)
    {
        Button button = gameObject.GetComponent<Button>();
        button.name = level.ToString();
        buttonText.text = level.ToString();
        button.onClick.AddListener(() => OnClickLevel(level));
    }

    public void OnClickLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
