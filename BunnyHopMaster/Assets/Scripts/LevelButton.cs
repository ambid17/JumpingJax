using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelButton : MonoBehaviour
{
    public Text buttonText;

    public void SetupButton(int level, bool isLevelCompleted, string levelName)
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.name = levelName;

        TimeSpan time = TimeSpan.FromSeconds(PlayerStatsManager.GetLevelCompletion(level));

        buttonText.text = levelName + "\n" + time.ToString("hh':'mm':'ss");
        button.onClick.AddListener(() => OnClickLevel(level));
    }

    public void OnClickLevel(int level)
    {
        GameManager._GameManager.currentLevel = level;
        SceneManager.LoadScene(level);
    }
}
