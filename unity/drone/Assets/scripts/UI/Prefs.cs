using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefs : MonoBehaviour
{
    public InputField[] InputFields;
    public Dropdown[] Dropdowns;
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
        foreach (InputField input in InputFields) if (PlayerPrefs.HasKey(input.name)) input.text = PlayerPrefs.GetString(input.name);
        foreach (Dropdown dropdown in Dropdowns) if (PlayerPrefs.HasKey(dropdown.name)) dropdown.value = PlayerPrefs.GetInt(dropdown.name);
    }
    public void SetPlayerPrefs()
    {
        foreach (InputField input in InputFields) PlayerPrefs.SetString(input.name, input.text);
        foreach (Dropdown dropdown in Dropdowns) PlayerPrefs.SetInt(dropdown.name, dropdown.value);
    }
    public void SavePlayerPrefs()
    {
        SetPlayerPrefs();
        PlayerPrefs.Save();
    }
}
