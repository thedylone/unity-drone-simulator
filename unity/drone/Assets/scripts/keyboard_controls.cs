using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboard_controls : MonoBehaviour
{
    // Start is called before the first frame update
    float speed;
    void Start()
    {
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody drone = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.W)){
            drone.AddForce(Vector3.forward * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S)){
            drone.AddForce(Vector3.back * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A)){
            drone.AddForce(Vector3.left * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D)){
            drone.AddForce(Vector3.right * speed, ForceMode.Force);
        }
    }
}
