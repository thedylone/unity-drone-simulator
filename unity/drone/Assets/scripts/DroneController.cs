using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject Drone = Settings.DroneBModel;
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
            Drone = Instantiate(Settings.DroneBModel);
            Drone.transform.parent = DroneGroup.transform;
            Drone.transform.localPosition = new Vector3(0, 55 - Settings.DroneDistance, 0);
            Drone.AddComponent<Rigidbody>().useGravity = false;
            Drone.AddComponent<MeshRenderer>();
            MaxSpeed = Settings.DroneBSpeed;
        }
    }
}
