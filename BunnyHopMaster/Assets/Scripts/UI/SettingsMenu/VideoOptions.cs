using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown fullScreenDropdowne;
    public Dropdown graphicsQualityDropdown;

    Resolution[] resolutions;

    void Start()
    {
        SetupResolutionDropdown();
        SetupGraphicsDropdown();
    }

    public void SetDefaults()
    {

    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
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
    }

    void SetupGraphicsDropdown()
    {
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        graphicsQualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
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
        PlayerPrefs.SetInt("Quality", qualityIndex);
        OptionsPreferencesManager.SetQuality(qualityIndex);
    }
}
