using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public bool EnableHover = false;
    public float HoverDistance = 10f;
    public int Layer = 3;
    // public VelocityConverter converter;
    public bool UseConverter;

    GameObject drone;
    float speed;

    void Update()
    {
        RaycastHit hit;

        speed = GetComponent<DroneController>().MaxSpeed;
        drone = GetComponent<DroneController>().Drone;

        Rigidbody rb = drone.GetComponent<Rigidbody>();

        int layerMask = 1 << Layer;

        if (EnableHover)
        {
            // keep target at a fixed distance above objects below it regardless of height
            rb.useGravity = true;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, HoverDistance, layerMask))
            {
                transform.Translate(0, (HoverDistance - hit.distance), 0);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (UseConverter)
        {
            // converter.Convert(horizontalInput, verticalInput);
            VelocityConverter.Convert(rb, horizontalInput, verticalInput, GetComponent<DroneController>().MaxSpeed, 25, 1);
        }
        else
        {
            rb.AddForce(horizontalInput * speed, 0, verticalInput * speed);
        }
    }
}