using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderItem : MonoBehaviour
{
    public Text label;
    public Slider slider;
    public InputField input;

    public void Init(string labelText, float value, UnityAction<float> setSensitivity, float minValue, float maxValue, bool isInt)
    {
        label.text = labelText;

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(setSensitivity);
        slider.value = value;
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.wholeNumbers = isInt;
        input.text = value.ToString();
    }

    public void SetLabel(string text)
    {
        this.label.text = text;
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetInput(string text)
    {
        input.text = text;
    }
}
