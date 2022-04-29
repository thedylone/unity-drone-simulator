using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    public Slider Slider;
    public InputField InputField;
    public void UpdateInputFromSlider(float sliderValue)
    {
        InputField.text = sliderValue.ToString("f1");
    }

    public void UpdateSliderFromInput(string inputValue)
    {
        float _inputValue = Mathf.Clamp(float.Parse(inputValue), Slider.minValue, Slider.maxValue);
        InputField.text = _inputValue.ToString();
        Slider.value = _inputValue;
    }
}
