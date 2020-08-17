using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject sliderPrefab;
    public Transform scrollViewContent;
    private SliderItem volume;

    private const string musicVolumeParam = "MusicVolume";

    void Start()
    {
        InitializeVolume();
    }

    public void SetDefaults()
    {
        OptionsPreferencesManager.SetVolume(OptionsPreferencesManager.defaultVolume);
        volume.slider.value = ConvertFromDecibel(OptionsPreferencesManager.defaultVolume);
    }

    public void SetVolume(float value)
    {
        volume.input.text = (int)(value * 100) + "%";

        float volumeInDecibels = ConvertToDecibel(value);
        audioMixer.SetFloat(musicVolumeParam, volumeInDecibels);
        OptionsPreferencesManager.SetVolume(volumeInDecibels);
    }

    public void InitializeVolume()
    {
        if(volume == null)
        {
            GameObject newSlider = Instantiate(sliderPrefab, scrollViewContent);
            volume = newSlider.GetComponent<SliderItem>();
        }
        volume.Init("Volume", ConvertFromDecibel(OptionsPreferencesManager.GetVolume()), SetVolume, 0.0001f, 1, false);
        volume.input.text = (int) (ConvertFromDecibel(OptionsPreferencesManager.GetVolume()) * 100) + "%";

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
