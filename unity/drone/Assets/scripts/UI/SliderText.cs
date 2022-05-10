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
        // sets input field to slider value to 1 d.p. when slider is changed
        InputField.text = sliderValue.ToString("f1");
    }

    public void UpdateSliderFromInput(string inputValue)
    {
        // sets slider value to input field value, bounded by slider's min and max
        float _inputValue = Mathf.Clamp(float.Parse(inputValue), Slider.minValue, Slider.maxValue);
        InputField.text = _inputValue.ToString();
        Slider.value = _inputValue;
    }
    void Start()
    {
        Slider.value = float.Parse(InputField.text);
    }
}
