using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZmqUI : MonoBehaviour
{
    public InputField PathField;

    void Start()
    {
        Settings.ZmqPath = PathField.text;
        PathField.onValueChanged.AddListener(pathChange);
    }
    void pathChange(string path)
    {
        Settings.ZmqPath = path;
    }
}
