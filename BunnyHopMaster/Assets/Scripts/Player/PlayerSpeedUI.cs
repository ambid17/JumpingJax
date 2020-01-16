using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedUI : MonoBehaviour
{
    public Movement playerMovement;

    /*print() style */
    public GUIStyle style;

    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
    }
    private void OnGUI()
    {
        var velocity = playerMovement._newVelocity;
        velocity.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(velocity.magnitude * 100) / 100 + "m/s", style);
    }
}
