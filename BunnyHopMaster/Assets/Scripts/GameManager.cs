using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager;
    public AudioSource musicSource;

    public int currentLevel;
    public float completionTime;

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (GameManager._GameManager == null)
        {
            GameManager._GameManager = this;
        }
        else if (GameManager._GameManager == this)
        {
            Destroy(GameManager._GameManager.gameObject);
            GameManager._GameManager = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        completionTime = 0;
    }

    void Update()
    {
        completionTime += Time.deltaTime;
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
