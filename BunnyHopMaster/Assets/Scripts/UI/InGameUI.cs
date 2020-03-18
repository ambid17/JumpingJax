using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Text completionTimeText;
    public Text fpsText;
    public Text speedText;
    public Text tutorialText;
    public GameObject tutorialPane;
    public PlayerMovement playerMovement;

    private string[] tutorialTexts;
    private int tutorialTextIndex = 0;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

        tutorialTexts = GameManager.GetCurrentLevel().tutorialTexts;
        LoadNextTutorial();
    }

    void Update()
    {
        if(GameManager.Instance != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.currentCompletionTime);
            completionTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
        }

        fpsText.text = "FPS: " + Mathf.Round(1 / Time.deltaTime);
        Vector2 directionalSpeed = new Vector2(playerMovement.newVelocity.x, playerMovement.newVelocity.z);
        speedText.text = "Speed: " + Mathf.Round(directionalSpeed.magnitude * 100) / 100 + "m/s";

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LoadNextTutorial();
        }

    }

    private void LoadNextTutorial()
    {
        if(tutorialTextIndex < tutorialTexts.Length)
        {
            tutorialPane.SetActive(true);
            tutorialText.text = tutorialTexts[tutorialTextIndex];
            tutorialTextIndex++;
        }
        else
        {
            tutorialPane.SetActive(false);
        }
    }
}
