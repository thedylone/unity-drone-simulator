using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    public GameObject[] GameObjects;
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;
    void Start()
    {
        initialPositions = new Vector3[GameObjects.Length];
        initialRotations = new Quaternion[GameObjects.Length];
        for (int i = 0; i < GameObjects.Length; i++)
        {
            initialPositions[i] = GameObjects[i].transform.position;
            initialRotations[i] = GameObjects[i].transform.rotation;
        }
    }

    // Update is called once per frame
    public void Restart()
    {
        for (int i = 0; i < GameObjects.Length; i++)
        {
            GameObjects[i].transform.position = initialPositions[i];
            GameObjects[i].transform.rotation = initialRotations[i];
            GameObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObjects[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
