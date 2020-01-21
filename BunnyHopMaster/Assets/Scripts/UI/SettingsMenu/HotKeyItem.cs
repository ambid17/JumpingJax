using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyItem : MonoBehaviour
{
    public Text itemText;
    public Button itemButton;
    public Text buttonText;
    
    public void SetItemText(string text)
    {
        itemText.text = text;
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }

    public Text GetButtonText()
    {
        return buttonText;
    }
}
