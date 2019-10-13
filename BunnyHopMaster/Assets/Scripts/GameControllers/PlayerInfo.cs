using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int numberOfLevels;
    public Level[] levels;

    void Awake()
    {
        if(PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else if (PlayerInfo.PI == this)
        {
            Destroy(PlayerInfo.PI.gameObject);
            PlayerInfo.PI = this;
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
}
