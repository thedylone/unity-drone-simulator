using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class RtspUI : MonoBehaviour
{
    public InputField PortField;
    public InputField UrlField;
    public Text OutputText;

    void Start()
    {
        updateOutputText();
        PortField.onValueChanged.AddListener(delegate
        {
            PortChange();
        });
        UrlField.onValueChanged.AddListener(delegate
        {
            UrlChange();
        });
    }
    public void PortChange()
    {
        // code to change rtsp settings
        updateOutputText();
    }

    public void UrlChange()
    {
        // code to change rtsp settings
        updateOutputText();
    }

    void updateOutputText()
    {
        string[] ipAddressList = GetLocalIPAddresses();
        OutputText.text = "Connect to the RTSP stream:";
        if (ipAddressList.Length == 0)
        {
            OutputText.text += "\nNo IP Addresses found";
        }
        else
        {
            foreach (string ip in ipAddressList)
            {
                OutputText.text += "\nrtsp://" + ip + ":" + PortField.text + "/" + UrlField.text;
            }
        }
    }

    public static string[] GetLocalIPAddresses()
    {
        List<string> ipAddressList = new List<string>();
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressList.Add(ip.ToString());
            }
        }
        return ipAddressList.ToArray();
    }
}
