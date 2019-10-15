using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public CharacterController _controller;

    /*print() style */
    public GUIStyle style;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        _controller = GetComponent<CharacterController>();
    }
    private void OnGUI()
    {
        var ups = _controller.velocity;
        ups.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "m/s", style);
        GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerMovement.playerTopVelocity * 100) / 100 + "m/s", style);
    }
}
