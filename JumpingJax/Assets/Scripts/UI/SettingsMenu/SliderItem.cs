using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderItem : MonoBehaviour
{
    public Text text;
    public Slider slider;
    public InputField input;

    public void Init(string labelText, float value, UnityAction<float> setSensitivity)
    {
        text.text = labelText;

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(setSensitivity);
        slider.value = value;
        slider.minValue = 0;
        slider.maxValue = 1;

        input.text = value.ToString();
    }

    public void SetLabel(string text)
    {
        this.text.text = text;
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
