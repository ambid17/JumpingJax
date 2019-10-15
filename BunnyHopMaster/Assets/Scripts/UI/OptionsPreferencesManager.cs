using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPreferencesManager
{
    public static int GetResolutionWidth()
    {
        return PlayerPrefs.GetInt("ResolutionWidth", 1920);
    }

    public static int GetResolutionHeight()
    {
        return PlayerPrefs.GetInt("ResolutionHeight", 1080);
    }

    public static void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
    }

    public static float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", -20);
    }

    public static void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public static int GetQuality()
    {
        return PlayerPrefs.GetInt("Quality", 0);
    }

    public static void SetQuality(int quality)
    {
        PlayerPrefs.SetInt("Quality", quality);
    }

    public static bool GetFullScreen()
    {
        int isFullScreen = PlayerPrefs.GetInt("IsFullScreen", 0);
        return isFullScreen == 0 ? false : true;
    }

    public static void SetFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 50);
    }

    public static void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
    }
}
