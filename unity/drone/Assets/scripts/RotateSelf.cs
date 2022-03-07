using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public float xSpeed, ySpeed, zSpeed;

    void Awake()
    {

    }

    void Update()
    {
        transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime, Space.Self);
    }
}
