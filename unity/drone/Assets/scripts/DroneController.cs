using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject Drone;
    public GameObject DroneGroup;
    public float MaxSpeed;
    void Awake()
    {
        if (gameObject.name == "Drone A controller")
        {
            MaxSpeed = Settings.DroneASpeed;
        }
        else
        {
            MaxSpeed = Settings.DroneBSpeed;
            // instantiate drone B and set up properties
            Drone = Instantiate(Settings.DroneBModel);
            Drone.transform.parent = DroneGroup.transform;
            Drone.transform.localPosition = new Vector3(0, 55 - Settings.DroneDistance, 0);
            Drone.AddComponent<Rigidbody>();
            Drone.GetComponent<Rigidbody>().useGravity = false;
            Drone.AddComponent<MeshRenderer>();
        }
    }
}
