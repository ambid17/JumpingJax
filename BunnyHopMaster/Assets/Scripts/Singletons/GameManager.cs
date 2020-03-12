using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public AudioSource musicSource;
    public LevelDataContainer levelDataContainer;

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
    }

    void Update()
    {
        currentCompletionTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        musicSource.Stop();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        musicSource.Play();
        currentLevelBuildIndex = scene.buildIndex;
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
    }
}
