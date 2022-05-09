using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZmqUI : MonoBehaviour
{
    public InputField PathField;
    public InputField DelayField;
    public Text OutputText;

    void Start()
    {
        pathChange(PathField.text);
        delayChange(DelayField.text);
        PathField.onValueChanged.AddListener(pathChange);
        DelayField.onValueChanged.AddListener(delayChange);
    }
    void pathChange(string path)
    {
        Settings.ZmqPath = path;
        OutputText.text = "ZeroMQ will connect to:\n" + path;
    }
    void delayChange(string delay)
    {
        Settings.ZmqDelay = float.Parse(delay);
    }
}
