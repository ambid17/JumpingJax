using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    public GameObject dropdownItemPrefab;
    public GameObject sliderItemPrefab;

    private DropdownItem resolutionDropdown;
    private DropdownItem fullScreenDropdown;
    private DropdownItem graphicsQualityDropdown;

    private SliderItem cameraFOV;

    private Transform scrollViewContent;

    Resolution[] resolutions;

    private Camera playerCamera;

    void Start()
    {
        scrollViewContent = GetComponentInChildren<ContentSizeFitter>().transform;
        SetupResolutionDropdown();
        SetupGraphicsDropdown();
        SetupFullscreenDropdown();
        SetupCameraFOV();
    }

    public void SetDefaults()
    {
        SetDefaultResolution();
        SetDefaultGraphics();
        SetDefaultFullscreen();
        SetDefaultCameraFOV();
    }

    void SetupResolutionDropdown()
    {
        resolutions = GetBestResolutions();
        if(resolutionDropdown == null)
        {
            GameObject newDropdown = Instantiate(dropdownItemPrefab, scrollViewContent);
            resolutionDropdown = newDropdown.GetComponent<DropdownItem>();
        }

        resolutionDropdown.Init("Resolution", GetStartingResolution(), GetResolutionCapabilities(), SetResolution);
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
        if (graphicsQualityDropdown == null)
        {
            GameObject newDropdown = Instantiate(dropdownItemPrefab, scrollViewContent);
            graphicsQualityDropdown = newDropdown.GetComponent<DropdownItem>();
        }
        graphicsQualityDropdown.Init("Quality", QualitySettings.GetQualityLevel(), QualitySettings.names.ToList(), SetQuality);
    }

    void SetupFullscreenDropdown()
    {
        if (fullScreenDropdown == null)
        {
            GameObject newDropdown = Instantiate(dropdownItemPrefab, scrollViewContent);
            fullScreenDropdown = newDropdown.GetComponent<DropdownItem>();
        }
        fullScreenDropdown.Init("Fullscreen", 0, new List<string> {"FullScreen", "Windowed"}, SetFullScreen);
    }

    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, OptionsPreferencesManager.GetFullScreen());
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

    public void SetupCameraFOV()
    {
        if (cameraFOV == null)
        {
            GameObject newDropdown = Instantiate(sliderItemPrefab, scrollViewContent);
            cameraFOV = newDropdown.GetComponent<SliderItem>();
        }
        cameraFOV.Init("Field Of View", OptionsPreferencesManager.GetCameraFOV(), SetCameraFOV, 45, 130, true);

        CameraMove cameraMove = GetComponentInParent<CameraMove>();
        if (cameraMove != null)
        {
            playerCamera = cameraMove.GetComponentInChildren<Camera>();
        }
    }

    public void SetCameraFOV(float value)
    {
        int newValue = Mathf.FloorToInt(value);
        cameraFOV.input.text = newValue.ToString();
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
        resolutionDropdown.dropdown.value = resolutions.Length - 1;
    }

    private void SetDefaultGraphics()
    {
        // Set graphics to the lowest supported to prevent hardware issues
        graphicsQualityDropdown.dropdown.value = 0;
    }

    private void SetDefaultFullscreen()
    {
        fullScreenDropdown.dropdown.value = 1;
    }

    private void SetDefaultCameraFOV()
    {
        Debug.Log(OptionsPreferencesManager.defaultCameraFOV);
        cameraFOV.slider.value = OptionsPreferencesManager.defaultCameraFOV;
    }
}