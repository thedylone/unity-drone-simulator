using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeyboardController : MonoBehaviour
{
    public float Speed = 1f;
    public bool EnableHover = false;
    public float HoverDistance = 10f;
    public int Layer = 3;
    public DirectVelocity converter;

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

        if (EnableHover)
        {
            rb.useGravity = true;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, HoverDistance, layerMask))
            {
                transform.Translate(0, (HoverDistance - hit.distance), 0);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
        float horizontalInput = Input.GetAxis("Keyboard Horizontal");
        float verticalInput = Input.GetAxis("Keyboard Vertical");

        if (converter)
        {
            converter.Convert(horizontalInput, verticalInput);
        }
        else
        {
            rb.AddForce(horizontalInput * Speed, 0, verticalInput * Speed);
        }
    }
}