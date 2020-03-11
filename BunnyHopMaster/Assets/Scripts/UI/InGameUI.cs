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
    public PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if(GameManager.Instance != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.currentCompletionTime);
            completionTimeText.text = "Time: " + time.ToString("hh':'mm':'ss");
        }

        fpsText.text = "FPS: " + 1 / Time.deltaTime;
        speedText.text = "Speed: " + Mathf.Round(playerMovement.newVelocity.magnitude * 100) / 100 + "m/s";

    }
}
