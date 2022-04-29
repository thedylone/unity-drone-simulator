using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class RtspUI : MonoBehaviour
{
    public InputField PortField;
    public InputField UrlField;
    public Text[] OutputTextList;
    public Toggle TimestampToggle;
    public InputField WidthField;
    public InputField HeightField;
    public InputField FrameRateField;

    void Start()
    {
        Settings.RtspPort = PortField.text;
        Settings.RtspUrl = UrlField.text;
        Settings.TimestampToggle = TimestampToggle.isOn;
        Settings.RtspWidth = int.Parse(WidthField.text);
        Settings.RtspHeight = int.Parse(HeightField.text);
        Settings.RtspFrameRate = int.Parse(FrameRateField.text);
        updateOutputText();
        PortField.onValueChanged.AddListener(portChange);
        UrlField.onValueChanged.AddListener(urlChange);
        TimestampToggle.onValueChanged.AddListener(timestampChange);
        WidthField.onValueChanged.AddListener(widthChange);
        HeightField.onValueChanged.AddListener(heightChange);
        FrameRateField.onValueChanged.AddListener(frameRateChange);
    }
    void portChange(string port)
    {
        Settings.RtspPort = port;
        updateOutputText();
    }

    void urlChange(string url)
    {
        Settings.RtspUrl = url;
        updateOutputText();
    }

    void timestampChange(bool isOn)
    {
        Settings.TimestampToggle = isOn;
    }
    void widthChange(string width)
    {
        Settings.RtspWidth = int.Parse(width);
    }
    void heightChange(string height)
    {
        Settings.RtspHeight = int.Parse(height);
    }
    void frameRateChange(string frameRate)
    {
        Settings.RtspFrameRate = int.Parse(frameRate);
    }
    void updateOutputText()
    {
        // string[] ipAddressList = GetLocalIPAddresses();
        NetworkInterfaceIP[] ipAddressList = GetLocalIPAddresses();
        string output = "Connect to the RTSP stream:";
        if (ipAddressList.Length == 0)
        {
            output += "\nNo IP Addresses found";
        }
        else
        {
            foreach (NetworkInterfaceIP ip in ipAddressList)
            {
                output += "\n" + ip.Nic.Name + " - rtsp://" + ip.Ip.Address.ToString() + ":" + PortField.text + "/" + UrlField.text;
            }
        }
        foreach (Text OutputText in OutputTextList)
        {
            OutputText.text = output;
        }
    }

    // public static string[] GetLocalIPAddresses()
    public static NetworkInterfaceIP[] GetLocalIPAddresses()
    {
        List<string> ipAddressList = new List<string>();
        List<NetworkInterfaceIP> niIPList = new List<NetworkInterfaceIP>();
        var host = Dns.GetHostEntry(Dns.GetHostName());
        // foreach (var ip in host.AddressList)
        // {
        //     if (ip.AddressFamily == AddressFamily.InterNetwork)
        //     {
        //         ipAddressList.Add(ip.ToString());
        //     }
        // }
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if ((nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet) && nic.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddressList.Add(nic.Name + " " + ip.Address.ToString());
                        niIPList.Add(new NetworkInterfaceIP(nic, ip));
                    }
                }
            }
        }
        // return ipAddressList.ToArray();
        return niIPList.ToArray();
    }
}

public class NetworkInterfaceIP
{
    public NetworkInterfaceIP(NetworkInterface networkInterface, UnicastIPAddressInformation unicastIPAddressInformation)
    {
        Nic = networkInterface;
        Ip = unicastIPAddressInformation;
    }

    public NetworkInterface Nic { get; }
    public UnicastIPAddressInformation Ip { get; }
}