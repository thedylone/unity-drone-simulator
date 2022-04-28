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
        DroneControllers = GameObject.FindGameObjectsWithTag("GameController");
        s_initialPositions = new Vector3[DroneControllers.Length];
        s_initialRotations = new Quaternion[DroneControllers.Length];
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            s_initialPositions[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.position;
            s_initialRotations[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation;
        }
    }
    public static void Restart()
    {
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.position = s_initialPositions[i];
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation = s_initialRotations[i];
        }
    }
}
