using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static bool GetKeyDown(string keyName)
    {
        if (Input.GetKeyDown(HotKeyManager.Instance.GetKeyFor(keyName)))
        {
            return true;
        }

        return false;
    }

    public static bool GetKey(string keyName)
    {
        if (Input.GetKey(HotKeyManager.Instance.GetKeyFor(keyName)))
        {
            return true;
        }

        return false;
    }
}
