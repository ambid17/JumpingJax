using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioSource musicSource;
    public LevelDataContainer levelDataContainer;

    public int currentLevelIndex;
    public float currentCompletionTime;

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

    private void OnLevelWasLoaded(int level)
    {
        currentCompletionTime = 0;
    }

    void Update()
    {
        currentCompletionTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        musicSource.Play();
    }

    private void OnDisable()
    {
        musicSource.Stop();
    }
}
