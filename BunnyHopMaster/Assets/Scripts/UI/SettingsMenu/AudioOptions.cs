using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Text volumeValue;

    private const string musicVolumeParam = "MusicVolume";

    void Start()
    {
        float volume = OptionsPreferencesManager.GetVolume();
        InitializeVolume(volume);
    }

    public void SetDefaults()
    {

    }

    public void SetVolume(float volume)
    {
        volumeValue.text = (int)(volume * 100) + "%";

        float volumeInDecibels = ConvertToDecibel(volume);
        audioMixer.SetFloat(musicVolumeParam, volumeInDecibels);
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
}
