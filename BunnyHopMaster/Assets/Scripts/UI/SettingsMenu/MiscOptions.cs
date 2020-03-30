using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiscOptions : MonoBehaviour
{
    public Text sensitivityValue;
    public Slider sensitivitySlider;

    CameraMove playerAiming;

    void Start()
    {
        PlayerMovement playerCharacter = GetComponentInParent<PlayerMovement>();
        if (playerCharacter != null)
        {
            playerAiming = playerCharacter.GetComponentInChildren<CameraMove>();
        }

        InitializeSensitivity();
    }

    public void SetDefaults()
    {
        sensitivitySlider.value = OptionsPreferencesManager.defaultSensitivity;
    }

    private void InitializeSensitivity()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        sensitivitySlider.value = OptionsPreferencesManager.GetSensitivity();
    }

    public void SetSensitivity(float sensitivity)
    {
        if (playerAiming != null)
        {
            playerAiming.sensitivityMultiplier = sensitivity;
        }

        int percentage = Mathf.RoundToInt(sensitivity * 100);
        sensitivityValue.text = percentage + "%";
        sensitivitySlider.value = sensitivity;
        Debug.Log("Setting sensitivity to: " + sensitivity);

        OptionsPreferencesManager.SetSensitivity(sensitivity);
    }
}
