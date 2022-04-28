using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestamp : MonoBehaviour
{
    public Text TimestampText;
    private bool _enabled;

    void Awake()
    {
        _enabled = Settings.TimestampToggle;
        GetComponent<Canvas>().enabled = _enabled;
    }
    void Start()
    {
        TimestampText.text = "timestamp: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled) TimestampText.text = "timestamp: " + Time.time;
    }
}
