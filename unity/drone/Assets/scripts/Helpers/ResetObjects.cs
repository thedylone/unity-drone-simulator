using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    public static GameObject[] DroneControllers;
    private static Vector3[] initialPositions;
    private static Quaternion[] initialRotations;
    void Start()
    {
        DroneControllers = GameObject.FindGameObjectsWithTag("GameController");
        initialPositions = new Vector3[DroneControllers.Length];
        initialRotations = new Quaternion[DroneControllers.Length];
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            initialPositions[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.position;
            initialRotations[i] = DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation;
        }
    }
    public static void Restart()
    {
        for (int i = 0; i < DroneControllers.Length; i++)
        {
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            DroneControllers[i].GetComponent<DroneController>().Drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.position = initialPositions[i];
            DroneControllers[i].GetComponent<DroneController>().Drone.transform.rotation = initialRotations[i];
        }
    }
}
