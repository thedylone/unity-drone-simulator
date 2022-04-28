using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestamp : MonoBehaviour
{
    public Text TimestampText;

    void Start()
    {
        TimestampText.text = "timestamp: 0";
    }

    // Update is called once per frame
    void Update()
    {
        TimestampText.text = "timestamp: " + Time.time;
    }
}
