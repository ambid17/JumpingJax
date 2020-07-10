using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelDataContainer levelDataContainer;
    public static uint AppId = 1315100;

    public int currentLevelBuildIndex;
    public float currentCompletionTime;
    public bool didWinCurrentLevel;

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
        else if (GameManager.Instance == this)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        StartSteam();
    }

    private void StartSteam()
    {
        try
        {
            if (!SteamClient.IsValid)
            {
                SteamClient.Init(AppId, true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not connect to steam " + e.Message);
        }
    }

    void Update()
    {
        if (!didWinCurrentLevel)
        {
            currentCompletionTime += Time.deltaTime;
        }
    }

    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevelBuildIndex = scene.buildIndex;

        if (scene.buildIndex == -1)
        {
            // This means the scene is loaded from outside of the menu, possibly from the workshop
            // TODO: update this based off of the loaded level
            currentLevelBuildIndex = 1;
        }

        currentCompletionTime = 0;
        didWinCurrentLevel = false;
    }

    public static Level GetCurrentLevel()
    {
        return Instance.levelDataContainer.levels[Instance.currentLevelBuildIndex - 1];
    }

    public static void FinishedLevel()
    {
        Instance.didWinCurrentLevel = true;
        float completionTime = Instance.currentCompletionTime;
        Level levelToUpdate = GetCurrentLevel();

        levelToUpdate.isCompleted = true;

        if (completionTime < levelToUpdate.completionTime || levelToUpdate.completionTime == 0)
        {
            levelToUpdate.completionTime = completionTime;
        }

        StatsManager.SaveLevelCompletion(levelToUpdate.levelName, completionTime);
    }
}
