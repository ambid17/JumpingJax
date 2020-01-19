using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiscOptions : MonoBehaviour
{
    public Text sensitivityValue;
    public Slider sensitivitySlider;

    PlayerAiming playerAiming;

    void Start()
    {
        PlayerMovement playerCharacter = GetComponentInParent<PlayerMovement>();
        if (playerCharacter != null)
        {
            playerAiming = playerCharacter.GetComponentInChildren<PlayerAiming>();
        }

        float sensitivity = OptionsPreferencesManager.GetSensitivity();
        SetSensitivity(sensitivity);
    }

    public void SetDefaults()
    {

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
        Debug.Log("setsens: " + sensitivity);

        OptionsPreferencesManager.SetSensitivity(sensitivity);
    }
}
