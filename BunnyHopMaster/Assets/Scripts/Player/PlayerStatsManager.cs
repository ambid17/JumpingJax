using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager _PlayerStats;

    public int numberOfLevels;

    [SerializeField]
    public Level[] levels;

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (PlayerStatsManager._PlayerStats == null)
        {
            PlayerStatsManager._PlayerStats = this;
        }
        else if (PlayerStatsManager._PlayerStats == this)
        {
            Destroy(PlayerStatsManager._PlayerStats.gameObject);
            PlayerStatsManager._PlayerStats = this;
        }

        DontDestroyOnLoad(this.gameObject);

        InitializeStats();
    }

    private void OnLevelWasLoaded(int level)
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        numberOfLevels = SceneManager.sceneCountInBuildSettings - 1;
        levels = new Level[numberOfLevels];
        Scene s = SceneManager.GetSceneByBuildIndex(1);

        for (int i = 1; i <= numberOfLevels; i++)
        {
            string levelKey = "LevelCompletion" + i;
            float isCompleted = PlayerPrefs.GetFloat(levelKey, float.PositiveInfinity);
            levels[i - 1] = new Level(i, isCompleted, SceneManager.GetSceneByBuildIndex(i).name);
        }
    }

    public static void SetLevelCompletion(int level, float completionTime)
    {
        string completionKey = "LevelCompletion" + level;
        float currentBestTime = PlayerPrefs.GetFloat(completionKey, float.PositiveInfinity);
        if(completionTime < currentBestTime)
        {
            PlayerPrefs.SetFloat(completionKey, completionTime);
        }
    }

    public static float GetLevelCompletion(int level)
    {
        string completionKey = "LevelCompletion" + level;
        return PlayerPrefs.GetFloat(completionKey, 0);
    }
}
