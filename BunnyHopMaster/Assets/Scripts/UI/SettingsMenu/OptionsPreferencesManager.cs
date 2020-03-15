using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPreferencesManager
{
    private const string resolutionWidthKey = "ResolutionWidth";
    private const int defaultResolutionWidth = 1920;
    
    private const string resolutionHeightKey = "ResolutionHeight";
    private const int defaultResolutionHeight = 1080;

    private const string volumeKey = "Volume";
    public const int defaultVolume = -40;

    private const string qualityKey = "Quality";
    private const int defaultQuality = 0;

    private const string fullScreenKey = "IsFullScreen";
    private const int defaultIsFullScreen = 0;

    private const string sensitivityKey = "Sensitivity";
    private const float defaultSensitivity = 0.5f;

    private const string portalRecursionKey = "PortalRecursion";
    public const int defaultPortalRecursion = 5;

    public static int GetResolutionWidth()
    {
        return PlayerPrefs.GetInt(resolutionWidthKey, defaultResolutionWidth);
    }

    public static int GetResolutionHeight()
    {
        return PlayerPrefs.GetInt(resolutionWidthKey, defaultResolutionHeight);
    }

    public static void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt(resolutionWidthKey, width);
        PlayerPrefs.SetInt(resolutionHeightKey, height);
    }

    public static float GetVolume()
    {
        return PlayerPrefs.GetFloat(volumeKey, defaultVolume);
    }

    public static void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat(volumeKey, volume);
    }

    public static int GetQuality()
    {
        return PlayerPrefs.GetInt(qualityKey, defaultQuality);
    }

    public static void SetQuality(int quality)
    {
        PlayerPrefs.SetInt(qualityKey, quality);
    }

    public static bool GetFullScreen()
    {
        int isFullScreen = PlayerPrefs.GetInt(fullScreenKey, defaultIsFullScreen);
        return isFullScreen == 0 ? false : true;
    }

    public static void SetFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt(fullScreenKey, isFullScreen ? 1 : 0);
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(sensitivityKey, defaultSensitivity);
    }

    public static void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(sensitivityKey, sensitivity);
    }

    public static int GetPortalRecursion()
    {
        return PlayerPrefs.GetInt(portalRecursionKey, defaultPortalRecursion);
    }

    public static void SetPortalRecursion(int recursionLevel)
    {
        PlayerPrefs.SetInt(portalRecursionKey, recursionLevel);
    }
}
