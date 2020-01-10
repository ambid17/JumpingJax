using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField]
    public Checkpoint currentCheckpoint;
    public PlayerUI playerUI;
    public bool didWin = false;
    public float minimumY = 0;
    public string lastScenename;

    private void Start()
    {
        didWin = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if(gameObject.transform.position.y < minimumY) 
        {
            Respawn();
        }

        if(currentCheckpoint != null && !didWin)
        {
            if (currentCheckpoint.level == LevelData.LD.numberOfCheckpoints)
            {
                didWin = true;
                UpdatePlayerStats();
                playerUI.ShowWinScreen();
                Time.timeScale = 0;
            }
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(lastScenename) && didWin)
        {
            SceneManager.LoadScene("Credits");
        }
    }

    private void UpdatePlayerStats()
    {
        PlayerStatsManager.SetLevelCompletion(GameManager._GameManager.currentLevel, GameManager._GameManager.completionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkPointHit = other.gameObject.GetComponent<Checkpoint>();
        if (checkPointHit)
        {
            HitNewCheckPoint(checkPointHit);
        }
    }

    public void HitNewCheckPoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint == null)
        {
            checkpoint.SetCompleted();
            currentCheckpoint = checkpoint;
        }
        else
        {
            if (currentCheckpoint.level <= checkpoint.level)
            {
                checkpoint.SetCompleted();
                currentCheckpoint = checkpoint;
            }
        }
    }

    void Respawn()
    {
        Vector3 newPos = GetCurrentCheckpointPosition();
        newPos.y += 0.2f;
        transform.position = newPos;
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        return currentCheckpoint.gameObject.transform.position;
    }
}
