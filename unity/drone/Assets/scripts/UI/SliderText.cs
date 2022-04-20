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
        InputField.text = sliderValue.ToString("f3");
    }

    public void UpdateSliderFromInput(string InputValue)
    {
        Slider.value = float.Parse(InputValue);
    }
}
