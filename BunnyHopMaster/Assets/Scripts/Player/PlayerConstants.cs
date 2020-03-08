using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConstants
{
    public static float MoveSpeed = 10f;
    public static float CrouchingMoveSpeed = 5f;
    public static float MaxVelocity = 100f;

    public static float Gravity = 9.8f;
    public static float JumpPower = 5f;
    public static float CrouchingJumpPower = 3f;

    public static float GroundAcceleration = 7f;
    public static float AirAcceleration = 2.5f;

    public static float StopSpeed = 8f;
    public static float Friction = 6f;
    public static float normalSurfaceFriction = 1f;

    public static float AirAccelerationCap = 3f;

    public static Vector3 BoxCastExtents = new Vector3(0.3f, 0.9f, 0.3f);
    public static Vector3 CrouchingBoxCastExtents = new Vector3(0.3f, 0.5f, 0.3f);

    public static Vector3 BoxColliderSize = new Vector3(1f, 1.8f, 1f);
    public static Vector3 CrouchingBoxColliderSize = new Vector3(1f, 1f, 1f);

    public static float BoxCastDistance = 0.11f;
    public static float CrouchingBoxCastDistance = 0.11f;

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

    public static string MouseX = "Mouse X";
    public static string MouseY = "Mouse Y";

    public static int BuildSceneIndex = 0;
}
