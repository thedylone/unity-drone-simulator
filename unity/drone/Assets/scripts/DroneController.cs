using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject Drone;
    public float _MaxSpeed;
    public string MaxSpeed
    {
        get { return _MaxSpeed.ToString(); }
        set { _MaxSpeed = float.Parse(value); }
    }
}
