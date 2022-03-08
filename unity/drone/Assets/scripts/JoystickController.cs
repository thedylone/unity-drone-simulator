using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JoystickController : MonoBehaviour
{
    public float Speed = 1f;
    public bool EnableHover = false;
    public float HoverDistance = 10f;
    public int Layer = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Rigidbody rb = GetComponent<Rigidbody>();

        int layerMask = 1 << Layer;

        if (EnableHover && Physics.Raycast(transform.position, -Vector3.up, out hit, HoverDistance, layerMask))
        {
            transform.Translate(0, (HoverDistance - hit.distance), 0);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(horizontalInput * Speed, 0, verticalInput * Speed);
    }
}