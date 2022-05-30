using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    public static GameObject[] DroneControllers;
    private static Vector3[] s_initialPositions;
    private static Quaternion[] s_initialRotations;
    void Start()
    {
        // find Drone Controllers in scene. Note that Drone Controller GameObject should be tagged with "GameController"
        // for it to be recognised in this script
        DroneControllers = GameObject.FindGameObjectsWithTag("GameController");
        // create Arrays to store each Drone Controller's Drone Position and Rotation
        s_initialPositions = new Vector3[DroneControllers.Length];
        s_initialRotations = new Quaternion[DroneControllers.Length];
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            // save the Drone's initial position and rotation into the array
            s_initialPositions[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.position;
            s_initialRotations[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation;
        }
    }
    public static void Restart()
    {
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            // reset the drone's velocity and rotation speed to 0 first
            // this will stop the drone from moving/rotating away from the initial position/rotation
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            // then reset drone to its saved initial position and rotation
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.position = s_initialPositions[i];
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation = s_initialRotations[i];

            DroneControllers[i].GetComponent<VelocityConverter>().SetVelocities(0,0);
        }
    }
}
