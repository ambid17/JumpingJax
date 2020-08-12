using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderItem : MonoBehaviour
{
    public Text text;
    public Slider slider;
    public InputField input;

    public void SetItemText(string text)
    {
        this.text.text = text;
    }

    public void SetSliderValue(float value, float minValue, float maxValue)
    {
        slider.value = value;
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    public void SetInput(string text)
    {
        input.text = text;
    }
}
