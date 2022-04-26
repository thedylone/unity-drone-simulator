using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZmqUI : MonoBehaviour
{
    public InputField PathField;
    public Text OutputText;

    void Start()
    {
        pathChange(PathField.text);
        PathField.onValueChanged.AddListener(pathChange);
    }
    void pathChange(string path)
    {
        Settings.ZmqPath = path;
        OutputText.text = "ZeroMQ will connect to:\n" + path;
    }
}
