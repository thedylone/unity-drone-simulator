using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickGlance : MonoBehaviour
{
    public Toggle[] SettingsToggleList;
    void Start()
    {
        foreach (Toggle SettingsToggle in SettingsToggleList) SettingsToggle.onValueChanged.AddListener(toggleCanvas);
    }
    void toggleCanvas(bool value)
    {
        GetComponent<Canvas>().enabled = !value;
    }
}
