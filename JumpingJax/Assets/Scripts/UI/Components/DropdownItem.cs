using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropdownItem : MonoBehaviour
{
    public Text label;
    public Dropdown dropdown;

    public void Init(string labelText, int startValue, List<string> options, UnityAction<int> action)
    {
        label.text = labelText;
        dropdown.ClearOptions();

        dropdown.AddOptions(options);
        dropdown.value = startValue;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(action);
    }
}
