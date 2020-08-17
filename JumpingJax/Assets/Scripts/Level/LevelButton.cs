using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class LevelButton : MonoBehaviour
{
    public Text levelName;
    public Text levelTime;

    public void SetupButton(Level level, UnityAction action)
    {
        levelName.text = level.levelName;

        if (level.isCompleted)
        {
            TimeSpan time = TimeSpan.FromSeconds(level.completionTime);
            String timeString = time.ToString(PlayerConstants.levelCompletionTimeFormat);
            levelName.text = level.levelName + "\n" + timeString;
        }
        else
        {
            levelTime.text = "Not Completed";
        }

        Button button = GetComponentInChildren<Button>();
        button.name = level.levelName;
        button.onClick.AddListener(action);
    }
}
