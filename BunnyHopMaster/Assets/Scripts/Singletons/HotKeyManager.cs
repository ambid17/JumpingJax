using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyManager : MonoBehaviour {
    [SerializeField]
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> defaults = new Dictionary<string, KeyCode>();

    public static HotKeyManager Instance;
    

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitDefaults();
    }

    public void InitDefaults()
    {
        //TODO: get player prefs
        AddPlayerDefaultKey(PlayerConstants.Forward, PlayerConstants.ForwardDefault);
        AddPlayerDefaultKey(PlayerConstants.Back, PlayerConstants.BackDefault);
        AddPlayerDefaultKey(PlayerConstants.Left, PlayerConstants.LeftDefault);
        AddPlayerDefaultKey(PlayerConstants.Right, PlayerConstants.RightDefault);
        AddPlayerDefaultKey(PlayerConstants.Jump, PlayerConstants.JumpDefault);
        AddPlayerDefaultKey(PlayerConstants.Crouch, PlayerConstants.CrouchDefault);
        AddPlayerDefaultKey(PlayerConstants.ResetLevel, PlayerConstants.ResetLevelDefault);
        AddPlayerDefaultKey(PlayerConstants.Portal1, PlayerConstants.Portal1Default);
        AddPlayerDefaultKey(PlayerConstants.Portal2, PlayerConstants.Portal2Default);
        SetDefaults();
    }

    public void AddPlayerDefaultKey(string keyName, string defaultValue)
    {
        string key = PlayerPrefs.GetString(keyName, defaultValue);

        KeyCode keyCode;
        if (Enum.TryParse(key, out keyCode))
        {
            defaults.Add(keyName, keyCode);
        }
        else
        {
            Debug.Log("Could not parse key code: " + keyName);
        }
    }

    public void SetDefaults()
    {
        keys = defaults;
    }

    public Dictionary<String, KeyCode> GetHotKeys()
    {
        return keys;
    }

    public void SetButtonForKey(string key, KeyCode keyCode)
    {
        keys[key] = keyCode;
        PlayerPrefs.SetString(key, keyCode.ToString());
    }

    public KeyCode GetKeyFor(string action)
    {
        return keys[action];
    }
}

