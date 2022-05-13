using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    // rtsp settings
    public static string RtspPort;
    public static string RtspUrl;
    public static float RtspDelay; 
    public static bool TimestampToggle;
    public static int RtspWidth;
    public static int RtspHeight;
    public static int RtspFrameRate;

    // zmq settings
    public static string ZmqPath;
    public static float ZmqDelay;

    // drones settings
    public static float DroneDistance;
    public static float DroneASpeed;
    public static float DroneBSpeed;
    public static GameObject DroneBModel;
    
}
