using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyOptions : MonoBehaviour
{
    public Transform scrollViewContent;
    public GameObject hotKeySelectionPrefab;

    string keyToRebind = null;
    Dictionary<string, Text> buttonKeyCodeTexts;

    void Start()
    {
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
                        HotKeyManager.instance.SetButtonForKey(keyToRebind, keyCode);
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
        Dictionary<string, KeyCode> keys = HotKeyManager.instance.GetHotKeys();
        buttonKeyCodeTexts = new Dictionary<string, Text>();
        foreach(string hotkey in keys.Keys)
        {
            GameObject newItem = Instantiate(hotKeySelectionPrefab);
            newItem.transform.parent = scrollViewContent;

            HotKeyItem item = newItem.GetComponentInChildren<HotKeyItem>();
            item.SetItemText(hotkey);
            item.SetButtonText(keys[hotkey].ToString());
            item.itemButton.onClick.AddListener(() => StartRebindFor(hotkey.ToString()));

            buttonKeyCodeTexts.Add(hotkey, item.GetButtonText());
        }
    }

    public void SetDefaults()
    {
        HotKeyManager.instance.SetDefaults();
        ReloadUI();
    }
}
