using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyOptions : MonoBehaviour
{
    private Transform scrollViewContent;
    public GameObject hotKeySelectionPrefab;
    public GameObject sliderPrefab;


    string keyToRebind = null;
    Dictionary<string, Text> buttonKeyCodeTexts;
    CameraMove playerAiming;
    SliderItem currentSliderItem;

    void Start()
    {
        PlayerMovement playerCharacter = GetComponentInParent<PlayerMovement>();
        if (playerCharacter != null)
        {
            playerAiming = playerCharacter.GetComponentInChildren<CameraMove>();
        }

        scrollViewContent = GetComponentInChildren<ContentSizeFitter>().transform;
        ReloadUI();
    }

    private void Update()
    {
        if(keyToRebind != null)
        {
            if (Input.anyKeyDown)
            {
                // Loop through all possible keys and see if it was pressed down
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        HotKeyManager.Instance.SetButtonForKey(keyToRebind, keyCode);
                        buttonKeyCodeTexts[keyToRebind].text = keyCode.ToString();
                        keyToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    void ReloadUI()
    {
        CleanScrollView();
        PopulateHotkeys();
        SetupSensitivitySlider();
    }

    void StartRebindFor(string keyName)
    {
        Debug.Log("StartRebindFor: " + keyName);

        keyToRebind = keyName;
    }

    private void CleanScrollView()
    {
        foreach(Transform child in scrollViewContent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void PopulateHotkeys()
    {
        Dictionary<string, KeyCode> keys = HotKeyManager.Instance.GetHotKeys();
        buttonKeyCodeTexts = new Dictionary<string, Text>();
        foreach(string hotkey in keys.Keys)
        {
            GameObject newItem = Instantiate(hotKeySelectionPrefab);
            newItem.transform.SetParent(scrollViewContent, false);

            HotKeyItem item = newItem.GetComponentInChildren<HotKeyItem>();
            item.SetItemText(hotkey);
            item.SetButtonText(keys[hotkey].ToString());
            item.itemButton.onClick.AddListener(() => StartRebindFor(hotkey.ToString()));

            buttonKeyCodeTexts.Add(hotkey, item.GetButtonText());
        }
    }

    private void SetupSensitivitySlider()
    {
        GameObject sliderObject = Instantiate(sliderPrefab, scrollViewContent);
        currentSliderItem = sliderObject.GetComponent<SliderItem>();
        currentSliderItem.Init(OptionsPreferencesManager.sensitivityKey, OptionsPreferencesManager.GetSensitivity(), SetSensitivity);
    }

    public void SetSensitivity(float sensitivity)
    {
        if (playerAiming != null)
        {
            playerAiming.sensitivityMultiplier = sensitivity;
        }

        int percentage = Mathf.RoundToInt(sensitivity * 100);
        currentSliderItem.SetInput(percentage + "%");
        currentSliderItem.SetSliderValue(sensitivity);
        Debug.Log("Setting sensitivity to: " + sensitivity);

        OptionsPreferencesManager.SetSensitivity(sensitivity);
    }

    public void SetDefaults()
    {
        Debug.Log("set hotkey defaults");
        HotKeyManager.Instance.SetDefaults();
        ReloadUI();
    }
}
