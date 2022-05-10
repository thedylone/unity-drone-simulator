using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefs : MonoBehaviour
{
    public InputField[] InputFields;
    public Dropdown[] Dropdowns;
    public Toggle[] Toggles;
    void Awake()
    {
        LoadPlayerPrefs();
    }
    void OnApplicationQuit()
    {
        SetPlayerPrefs();
    }
    void OnDisable()
    {
        SavePlayerPrefs();
    }
    void OnDestroy()
    {
        SavePlayerPrefs();
    }
    public void LoadPlayerPrefs()
    {
        // retrieves player prefs from name
        foreach (InputField input in InputFields) if (PlayerPrefs.HasKey(input.name)) input.text = PlayerPrefs.GetString(input.name);
        foreach (Dropdown dropdown in Dropdowns) if (PlayerPrefs.HasKey(dropdown.name)) dropdown.value = PlayerPrefs.GetInt(dropdown.name);
        foreach (Toggle toggle in Toggles) if (PlayerPrefs.HasKey(toggle.name)) toggle.isOn = PlayerPrefs.GetInt(toggle.name) == 1 ? true : false;
    }
    public void SetPlayerPrefs()
    {
        // set player prefs based on the UI field's name
        foreach (InputField input in InputFields) PlayerPrefs.SetString(input.name, input.text);
        foreach (Dropdown dropdown in Dropdowns) PlayerPrefs.SetInt(dropdown.name, dropdown.value);
        foreach (Toggle toggle in Toggles) PlayerPrefs.SetInt(toggle.name, toggle.isOn ? 1 : 0);
    }
    public void SavePlayerPrefs()
    {
        SetPlayerPrefs();
        PlayerPrefs.Save();
    }
}
