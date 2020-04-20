using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown fullScreenDropdown;
    public Dropdown graphicsQualityDropdown;

    public Slider portalRecursionSlider;
    public Text portalRecursionText;

    public Slider cameraFOVSlider;
    public Text cameraFOVText;

    Resolution[] resolutions;

    public RecursivePortalCamera recursivePortalCamera;
    public GameObject mainCamera;
    public Camera playerCamera;

    void Start()
    {
        SetupResolutionDropdown();
        SetupGraphicsDropdown();
        SetupFullscreenDropdown();
        SetupPortalRecursion();
        SetupCameraFOV();
    }

    public void SetDefaults()
    {
        SetDefaultResolution();
        SetDefaultGraphics();
        SetDefaultFullscreen();
        SetDefaultPortalRecursion();
        SetDefaultCameraFOV();
    }

    void SetupResolutionDropdown()
    {
        resolutions = GetBestResolutions();
        resolutionDropdown.ClearOptions();

        resolutionDropdown.AddOptions(GetResolutionCapabilities());
        resolutionDropdown.value = GetStartingResolution();
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private List<string> GetResolutionCapabilities()
    {
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        return options;
    }

    private int GetStartingResolution()
    {
        int savedHeight = OptionsPreferencesManager.GetResolutionHeight();
        int savedWidth = OptionsPreferencesManager.GetResolutionWidth();
        int resolutionIndex = -1;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == savedWidth
                && resolutions[i].height == savedHeight)
            {
                resolutionIndex = i;
            }
        }

        // The default (1920 X 1080) wasnt found, return the highest available
        if (resolutionIndex == -1)
        {
            resolutionIndex = resolutions.Length - 1;
        }

        return resolutionIndex;
    }

    void SetupGraphicsDropdown()
    {
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        graphicsQualityDropdown.RefreshShownValue();

        graphicsQualityDropdown.onValueChanged.RemoveAllListeners();
        graphicsQualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    void SetupFullscreenDropdown()
    {
        fullScreenDropdown.onValueChanged.RemoveAllListeners();
        fullScreenDropdown.onValueChanged.AddListener(SetFullScreen);
    }

    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        OptionsPreferencesManager.SetResolution(resolution.width, resolution.height);
    }

    public void SetFullScreen(int isFullScreenSelection)
    {
        bool isFullScreen = isFullScreenSelection == 1 ? true : false;
        Screen.fullScreen = isFullScreen ? true : false;
        OptionsPreferencesManager.SetFullScreen(isFullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        OptionsPreferencesManager.SetQuality(qualityIndex);
    }

    public void SetupPortalRecursion()
    {
        portalRecursionSlider.onValueChanged.RemoveAllListeners();
        portalRecursionSlider.onValueChanged.AddListener(delegate
        { SetPortalRecursion(); });
        portalRecursionSlider.value = OptionsPreferencesManager.GetPortalRecursion();

        CameraMove cameraMove = GetComponentInParent<CameraMove>();
        if (cameraMove != null)
        {
            recursivePortalCamera = cameraMove.GetComponentInChildren<RecursivePortalCamera>();
        }
    }

    public void SetPortalRecursion()
    {
        int newValue = Mathf.FloorToInt(portalRecursionSlider.value);
        portalRecursionText.text = newValue.ToString();
        OptionsPreferencesManager.SetPortalRecursion(newValue);

        // This won't exist in the menu
        if (recursivePortalCamera != null)
        {
            recursivePortalCamera.UpdatePortalRecursion(newValue);
        }
    }

    public void SetupCameraFOV()
    {
        cameraFOVSlider.onValueChanged.RemoveAllListeners();
        cameraFOVSlider.onValueChanged.AddListener(delegate
        { SetCameraFOV(); });
        cameraFOVSlider.value = OptionsPreferencesManager.GetCameraFOV();

        CameraMove cameraMove = GetComponentInParent<CameraMove>();
        if (cameraMove != null)
        {
            playerCamera = cameraMove.GetComponentInChildren<Camera>();
        }
    }

    public void SetCameraFOV()
    {
        int newValue = Mathf.FloorToInt(cameraFOVSlider.value);
        cameraFOVText.text = newValue.ToString();
        OptionsPreferencesManager.SetCameraFOV(newValue);

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = newValue;
        }
    }
    // Get only the resolutions for the highest framerate
    private Resolution[] GetBestResolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        List<Resolution> bestResolutions = new List<Resolution>();

        int highestRefreshRate = 0;
        foreach (Resolution resolution in allResolutions)
        {
            if (resolution.refreshRate > highestRefreshRate)
            {
                highestRefreshRate = resolution.refreshRate;
            }
        }

        foreach (Resolution resolution in allResolutions)
        {
            if (resolution.refreshRate == highestRefreshRate)
            {
                bestResolutions.Add(resolution);
            }
        }

        return bestResolutions.ToArray();
    }

    private void SetDefaultResolution()
    {
        // Set resolution to the highest supported
        resolutionDropdown.value = resolutions.Length - 1;
    }

    private void SetDefaultGraphics()
    {
        // Set graphics to the lowest supported to prevent hardware issues
        graphicsQualityDropdown.value = 0;
    }

    private void SetDefaultFullscreen()
    {
        fullScreenDropdown.value = 1;
    }

    private void SetDefaultPortalRecursion()
    {
        Debug.Log(OptionsPreferencesManager.defaultPortalRecursion);
        portalRecursionSlider.value = OptionsPreferencesManager.defaultPortalRecursion;
    }
    private void SetDefaultCameraFOV()
    {
        Debug.Log(OptionsPreferencesManager.defaultCameraFOV);
        cameraFOVSlider.value = OptionsPreferencesManager.defaultCameraFOV;
    }
}