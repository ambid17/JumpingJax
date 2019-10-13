using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager _PlayerStats;

    public int numberOfLevels;
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
    }

    private void Start()
    {
        numberOfLevels = SceneManager.sceneCountInBuildSettings - 1;
        levels = new Level[numberOfLevels];

        for(int i = 1; i <= numberOfLevels; i++)
        {
            string levelKey = "Level" + i;
            if (PlayerPrefs.HasKey(levelKey))
            {
                int isCompleted = PlayerPrefs.GetInt(levelKey);
                levels[i - 1] = new Level(i, isCompleted);
            }
            else
            {
                PlayerPrefs.SetInt(levelKey, 0);
            }
        }
        
    }

    public static void SetLevelCompletion(int level, float completionTime)
    {
        string completionKey = "Complete" + level;
        float currentBestTime = PlayerPrefs.GetFloat(completionKey, float.PositiveInfinity);
        if(completionTime < currentBestTime)
        {
            PlayerPrefs.SetFloat(completionKey, completionTime);
        }
    }

    public static float GetLevelCompletion(int level)
    {
        string completionKey = "Complete" + level;
        return PlayerPrefs.GetFloat(completionKey, 0);
    }
}
