using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
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
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector3.back * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * speed, ForceMode.Force);
        }
    }
}
