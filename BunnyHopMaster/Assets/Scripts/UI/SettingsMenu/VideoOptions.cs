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

    Resolution[] resolutions;

    public RecursivePortalCamera recursivePortalCamera;

    void Start()
    {
        SetupResolutionDropdown();
        SetupGraphicsDropdown();
        SetupFullscreenDropdown();
        SetupPortalRecursion();
    }

    public void SetDefaults()
    {

    }

    void SetupResolutionDropdown()
    {
        resolutions = GetBestResolutions();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
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
        portalRecursionSlider.onValueChanged.AddListener(delegate { SetPortalRecursion(); }) ;
        portalRecursionSlider.value = OptionsPreferencesManager.GetPortalRecursion();

        CameraMove cameraMove = GetComponentInParent<CameraMove>();
        if(cameraMove != null)
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
        if(recursivePortalCamera != null)
        {
            recursivePortalCamera.UpdatePortalRecursion(newValue);
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
}
