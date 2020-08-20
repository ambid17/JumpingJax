using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConstants
{
    public static float MoveSpeed = 6f;
    public static float BackWardsMoveSpeedScale = 0.9f;
    public static float CrouchingMoveSpeed = 6f;
    public static float MaxVelocity = 30f;

    public static float Gravity = 20f;
    public static float JumpPower = 7.5f;
    public static float CrouchingJumpPower = 4f;
    public static float TimeBetweenJumps = 0.2f;

    public static float GroundAcceleration = 10f;
    public static float AirAcceleration = 1000f;
    public static float Overbounce = 1.001f;

    public static float StopSpeed = 6f;
    public static float Friction = 6f;
    public static float MinimumSpeedCutoff = 0.5f; // This is the speed after which the player is immediately stopped due to friction
    public static float NormalSurfaceFriction = 1f;

    public static float AirAccelerationCap = .7f;

    public static float StandingPlayerHeight = 1.6f;
    public static float CrouchingPlayerHeight = 0.8f;
    public static float TimeToCrouch = 0.5f;
    public static float groundCheckOffset = 0.05f;

    public static float portalWidth = 2f;
    public static float portalHeight = 2f;
    public static Vector3 PortalColliderExtents = new Vector3(1f, 1f, 1f);

    // Layer Masks
    public static LayerMask portalPlacementMask = new LayerMask();


    //HotKeys
    public static string Forward = "Forward";
    public static string ForwardDefault = "W";

    public static string Back = "Back";
    public static string BackDefault = "S";

    public static string Left = "Left";
    public static string LeftDefault = "A";

    public static string Right = "Right";
    public static string RightDefault = "D";

    public static string Jump = "Jump";
    public static string JumpDefault = "Space";

    public static string ResetLevel = "Reset Level";
    public static string ResetLevelDefault = "R";

    public static string Crouch = "Crouch";
    public static string CrouchDefault = "LeftControl";

    public static string Portal1 = "Portal1";
    public static string Portal1Default = "Mouse0";

    public static string Portal2 = "Portal2";
    public static string Portal2Default = "Mouse1";


    // Non-changeable hotkeys
    public static string MouseX = "Mouse X";
    public static string MouseY = "Mouse Y";
    public static KeyCode PauseMenu = KeyCode.Escape;
    public static KeyCode DebugMenu = KeyCode.BackQuote;
    public static KeyCode NextTutorial = KeyCode.Tab;

    public static KeyCode WinMenu_NextLevel = KeyCode.E;
    public static KeyCode WinMenu_RetryLevel = KeyCode.R;
    public static KeyCode WinMenu_MainMenu = KeyCode.Q;



    // Game Constants
    public static int BuildSceneIndex = 0;
    public static Vector3 PlayerSpawnOffset = new Vector3(0, 2, 0);
    public static string levelCompletionTimeFormat = "hh':'mm':'ss'.'fff";
    public static int PlayerLayer = 12;
    public static int PortalMaterialLayer = 10;
    public static int PortalLayer = 11;
}
