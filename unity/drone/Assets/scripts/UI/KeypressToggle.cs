using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypressToggle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
        }
    }
}
