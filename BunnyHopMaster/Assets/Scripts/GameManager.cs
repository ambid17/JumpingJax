using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public int currentLevel;
    public float completionTime;

    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (GameManager.GM == null)
        {
            GameManager.GM = this;
        }
        else if (GameManager.GM == this)
        {
            Destroy(GameManager.GM.gameObject);
            GameManager.GM = this;
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
}
