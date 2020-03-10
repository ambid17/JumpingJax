using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInitializer : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        float volume = OptionsPreferencesManager.GetVolume();
        Debug.Log("initVol:" + volume);
        InitializeVolume(volume);
    }

    public void SetDefaults()
    {

    }

    public void SetVolume(float volume)
    {
        float volumeInDecibels = ConvertToDecibel(volume);
        audioMixer.SetFloat("MusicVolume", volumeInDecibels);
        OptionsPreferencesManager.SetVolume(volumeInDecibels);
    }

    public void InitializeVolume(float volume)
    {
        SetVolume(ConvertFromDecibel(volume));
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
