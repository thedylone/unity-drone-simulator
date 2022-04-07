using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public bool EnableHover = false;
    public float HoverDistance = 10f;
    public int Layer = 3;
    public DirectVelocity converter;

    GameObject drone;
    float speed;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        speed = GetComponent<DroneController>()._MaxSpeed;
        drone = GetComponent<DroneController>().Drone;

        Rigidbody rb = drone.GetComponent<Rigidbody>();

        int layerMask = 1 << Layer;

        if (EnableHover)
        {
            rb.useGravity = true;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, HoverDistance, layerMask))
            {
                transform.Translate(0, (HoverDistance - hit.distance), 0);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (converter)
        {
            converter.Convert(horizontalInput, verticalInput);
        }
        else
        {
            rb.AddForce(horizontalInput * speed, 0, verticalInput * speed);
        }
    }
}