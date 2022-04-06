using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestamp : MonoBehaviour
{
    public Text m_TimestampText;

    void Start()
    {
        m_TimestampText.text = "timestamp: 0";
    }

    // Update is called once per frame
    void Update()
    {
        m_TimestampText.text = "timestamp: " + Time.time;
    }
}
