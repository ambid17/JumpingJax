using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static OptionsMenu;

public class TabButton : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public Image image;

    public void Init(string buttonText, UnityAction action)
    {
        this.buttonText.text = buttonText;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void SelectTab()
    {
        image.gameObject.SetActive(true);
    }

    public void UnselectTab()
    {
        image.gameObject.SetActive(false);
    }
}
