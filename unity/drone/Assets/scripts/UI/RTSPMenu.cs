using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSPMenu : MonoBehaviour
{
    public GameObject RTSPObject;
    public InputField PathInput;
    public void startRTSP()
    {
        RTSPObject.GetComponent<FFmpegOut.CameraCapture>().path = PathInput.text.ToString();
        RTSPObject.GetComponent<FFmpegOut.CameraCapture>().enabled = true;
    }

    public void stopRTSP()
    {
        RTSPObject.GetComponent<FFmpegOut.CameraCapture>().enabled = false;
    }
}
