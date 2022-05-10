using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    // meant for rotation of drone propellers
    public GameObject Prop1;
    public GameObject Prop2;
    public GameObject Prop3;
    public GameObject Prop4;
    private GameObject[] _props = new GameObject[4];

    public float xSpeed, ySpeed, zSpeed;

    void Awake()
    {
        _props[0] = Prop1;
        _props[1] = Prop2;
        _props[2] = Prop3;
        _props[3] = Prop4;
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_props[i] != null)
            {
                _props[i].transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime, Space.Self);
            }
        }

    }
}
