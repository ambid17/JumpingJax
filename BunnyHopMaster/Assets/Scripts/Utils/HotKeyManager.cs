using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HotKey {
    forward,
    backward,
    left,
    right,
    jump
}

public class HotKeyManager {
    private Dictionary<HotKey, KeyCode> keys = new Dictionary<HotKey, KeyCode>();

    public void set(HotKey key, KeyCode code) {
        keys[key] = code;
    }

    public bool check(HotKey key) {
        return Input.GetKey(keys[key]);
    }
}

