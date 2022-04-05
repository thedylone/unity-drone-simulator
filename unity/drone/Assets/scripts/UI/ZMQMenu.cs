using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZMQMenu : MonoBehaviour
{
    public GameObject ZMQObject;
    public InputField PathInput;
    public void startZMQ()
    {
        ZMQObject.GetComponent<ClientObject>().Path = PathInput.text.ToString();
        ZMQObject.GetComponent<ClientObject>().enabled = true;
    }

    public void stopZMQ()
    {
        ZMQObject.GetComponent<ClientObject>().enabled = false;
    }
}
