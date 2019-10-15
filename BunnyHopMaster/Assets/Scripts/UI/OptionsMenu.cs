using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    public Text sensitivityValue;

    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Text volumeValue;

    public Dropdown resolutionDropdown;

    public Dropdown graphicsQualityDropdown;
    public Toggle fullScreenToggle;

    Resolution[] resolutions;

    private void Start() {

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        float volume = OptionsPreferencesManager.GetVolume();
        InitializeVolume(volume);

        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        graphicsQualityDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;

        float sensitivity = OptionsPreferencesManager.GetSensitivity();
        SetSensitivity(sensitivity);
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        OptionsPreferencesManager.SetResolution(resolution.width, resolution.height);
    }

    public void SetVolume(float volume) {
        volumeValue.text = (int)(volume * 100) + "%";

        float volumeInDecibels = ConvertToDecibel(volume);
        audioMixer.SetFloat("MusicVolume", volumeInDecibels);
        OptionsPreferencesManager.SetVolume(volumeInDecibels);
    }
    
    public void InitializeVolume(float volume)
    {
        volumeSlider.value = ConvertFromDecibel(volume);
        SetVolume(volumeSlider.value);
    }

    public float ConvertToDecibel(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    public float ConvertFromDecibel(float value)
    {
        return Mathf.Pow(10, value / 20);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
        OptionsPreferencesManager.SetQuality(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
        OptionsPreferencesManager.SetFullScreen(isFullScreen);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.xMouseSensitivity = sensitivity;
            playerMovement.yMouseSensitivity = sensitivity;
        }

        sensitivityValue.text = sensitivity.ToString();

        OptionsPreferencesManager.SetSensitivity(sensitivity);
    }
}
