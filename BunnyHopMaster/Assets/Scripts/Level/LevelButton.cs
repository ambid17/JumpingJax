using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelButton : MonoBehaviour
{
    public Text buttonText;

    public void SetupButton(Level level)
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.name = level.levelName;
        Image backgroundImage = GetComponentInChildren<Image>();
        backgroundImage.color = level.isCompleted ? Color.green : Color.red;

        TimeSpan time = TimeSpan.FromSeconds(level.completionTime);
        String timeString = time.ToString("hh':'mm':'ss");
        buttonText.text = level.levelName + "\n" + timeString;
        button.onClick.AddListener(() => OnClickLevel(level.levelBuildIndex));
    }

    public void OnClickLevel(int sceneIndex)
    {
        GameManager.Instance.currentLevelIndex = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
