using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public float Speed = 1f;
    public float HoverDistance = 10f;
    public KeyCode Forward = KeyCode.W;
    public KeyCode Backward = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Rigidbody rb = GetComponent<Rigidbody>();

        int layerMask = 1 << 3;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, HoverDistance, layerMask))
        {
            transform.Translate(0, (HoverDistance - hit.distance), 0);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (Input.GetKey(Forward))
        {
            rb.AddForce(Vector3.forward * Speed, ForceMode.Force);
        }
        if (Input.GetKey(Backward))
        {
            rb.AddForce(Vector3.back * Speed, ForceMode.Force);
        }
        if (Input.GetKey(Left))
        {
            rb.AddForce(Vector3.left * Speed, ForceMode.Force);
        }
        if (Input.GetKey(Right))
        {
            rb.AddForce(Vector3.right * Speed, ForceMode.Force);
        }
    }
}
