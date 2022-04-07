using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefs : MonoBehaviour
{
    public InputField[] InputFields;
    
    void Start()
    {
        LoadPlayerPrefs();
    }
    void OnApplicationQuit()
    {
        SetPlayerPrefs();
    }
    public void LoadPlayerPrefs()
    {
        foreach (InputField input in InputFields)
        {
            if (PlayerPrefs.HasKey(input.name))
            {
                input.text = PlayerPrefs.GetString(input.name);
            }
        }
    }

    public void SetPlayerPrefs()
    {
        foreach (InputField input in InputFields)
        {
            PlayerPrefs.SetString(input.name, input.text);
        }
    }

    public void SavePlayerPrefs()
    {
        SetPlayerPrefs();
        PlayerPrefs.Save();
    }
}
