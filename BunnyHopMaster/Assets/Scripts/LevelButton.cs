using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelButton : MonoBehaviour
{
    public Text buttonText;

    public void SetupButton(int level, bool isLevelCompleted)
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.name = level.ToString();

        TimeSpan time = TimeSpan.FromSeconds(PlayerStatsManager.GetLevelCompletion(level));

        buttonText.text = level.ToString() + "\n" + time.ToString("hh':'mm':'ss");
        button.onClick.AddListener(() => OnClickLevel(level));
    }

    public void OnClickLevel(int level)
    {
        GameManager._GameManager.currentLevel = level;
        SceneManager.LoadScene(level);
    }
}
