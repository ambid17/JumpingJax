using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConstants
{
    public static float MoveSpeed = 10f;
    public static float MaxVelocity = 100f;

    public static float Gravity = 9.8f;
    public static float JumpPower = 5f;

    public static float GroundAcceleration = 7f;
    public static float AirAcceleration = 2.5f;

    public static float StopSpeed = 8f;
    public static float Friction = 6f;
    public static float normalSurfaceFriction = 1f;

    public static float AirAccelerationCap = 3f;

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
    public static string CrouchDefault = "Left Control";

    public static string Portal1 = "Portal1";
    public static string Portal1Default = "Mouse0";

    public static string Portal2 = "Portal2";
    public static string Portal2Default = "Mouse1";

    public static string MouseX = "Mouse X";
    public static string MouseY = "Mouse Y";
}
