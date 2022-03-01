using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public float Speed = 1f;
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
        Rigidbody rb = GetComponent<Rigidbody>();
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
