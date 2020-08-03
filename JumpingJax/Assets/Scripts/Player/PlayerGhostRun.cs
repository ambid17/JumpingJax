using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGhostRun : MonoBehaviour
{
    public GameObject ghostRunnerPrefab;
    private GameObject ghostRunner;

    private List<Vector3> currentRunData;
    private float ghostRunSaveTimer = 0;
    private float ghostRunnerTimer = 0;
    private Level currentLevel;
    private int currentDataIndex = 0;


    private const float ghostRunSaveInterval = 0.05f;


    void Start()
    {
        RestartRun();
        currentLevel = GameManager.GetCurrentLevel();
        if(ghostRunner == null)
        {
            ghostRunner = Instantiate(ghostRunnerPrefab);
        }
        ghostRunner.SetActive(false);
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        RecordCurrentRunData();
        UpdateGhost();
    }

    public void RestartRun()
    {
        ghostRunSaveTimer = 0;
        ghostRunnerTimer = 0;
        currentDataIndex = 0;
        currentRunData = new List<Vector3>();
    }

    private void UpdateGhost()
    {
        if (currentLevel == null 
            || currentLevel.ghostRun == null 
            || currentDataIndex >= currentLevel.ghostRun.Length - 1)
        {
            return; // ghost run is finished
        }

        // Only show the ghost run for a level we've completed
        if (currentLevel.isCompleted)
        {
            ghostRunner.SetActive(true);

            float lerpValue = ghostRunnerTimer / ghostRunSaveInterval;
            Vector3 position = Vector3.Lerp(ghostRunner.transform.position, currentLevel.ghostRun[currentDataIndex], lerpValue);
            ghostRunner.transform.position = position;
        }

        ghostRunnerTimer += Time.deltaTime;
        if (ghostRunnerTimer >= ghostRunSaveInterval)
        {
            currentDataIndex++;
            ghostRunnerTimer = 0;
        }
    }


    private void RecordCurrentRunData()
    {
        ghostRunSaveTimer += Time.deltaTime;
        if (ghostRunSaveTimer > ghostRunSaveInterval)
        {
            ghostRunSaveTimer = 0;
            currentRunData.Add(transform.position);
        }
    }

    public void SaveCurrentRunData()
    {
        GameManager.GetCurrentLevel().ghostRun = currentRunData.ToArray();
    }
}
