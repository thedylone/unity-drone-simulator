using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public GameObject prop1;
    public GameObject prop2;
    public GameObject prop3;
    public GameObject prop4;
    private GameObject[] props = new GameObject[4];

    public float xSpeed, ySpeed, zSpeed;

    void Awake()
    {
        
        props[0] = prop1;
        props[1] = prop2;
        props[2] = prop3;
        props[3] = prop4;
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (props[i] != null)
            {
                props[i].transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime, Space.Self);
            }
        }

    }
}
