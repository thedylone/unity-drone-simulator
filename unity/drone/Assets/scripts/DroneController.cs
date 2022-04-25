using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject Drone = Settings.DroneBModel;
    public float MaxSpeed;
    void Start()
    {
        if (gameObject.name == "Drone A controller")
        {
            MaxSpeed = Settings.DroneASpeed;
        }
        else
        {
            Drone = Instantiate(Settings.DroneBModel, new Vector3(0,15,0), new Quaternion(0,0,0,0));
            Drone.AddComponent<Rigidbody>().useGravity = false;
            MaxSpeed = Settings.DroneBSpeed;
        }
    }
}
