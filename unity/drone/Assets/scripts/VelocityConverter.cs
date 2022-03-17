using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class VelocityConverter : MonoBehaviour
{
    public float MaxSpeed = 15f;
    public float MaxTiltAngle = 25f;
    public float tiltSpeed = 1;
    public float Responsiveness = 25;
    [Range(-1, 1)]
    public float vx = 0.5f;
    [Range(-1, 1)]
    public float vy = 0.5f;

    private Rigidbody rb;
    private float mass;
    private float drag = 0.0280f;
    private float thrust;

    public void Convert(float vx, float vy)
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        rb.drag = 0;
        float ux = rb.velocity.x;
        float uy = rb.velocity.z;
        // prevent vx and vy from exceeding -1 to 1
        vx = Mathf.Min(Mathf.Abs(vx), 1) * Mathf.Sign(vx);
        vy = Mathf.Min(Mathf.Abs(vy), 1) * Mathf.Sign(vy);

        float tiltx = Mathf.Atan(Mathf.Tan(MaxTiltAngle * Mathf.PI / 180) * vx * vx) * Mathf.Sign(vx);
        float tilty = Mathf.Atan(Mathf.Tan(MaxTiltAngle * Mathf.PI / 180) * vy * vy) * Mathf.Sign(vy);
        // rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
        // transform.Rotate(tilty * 180 / Mathf.PI, 0, tiltx * 180 / Mathf.PI, Space.Self);
        rb.rotation = Quaternion.Euler(new Vector3(tilty * 180 / Mathf.PI, 0, -tiltx * 180 / Mathf.PI));
        float fx = mass * 9.81f * Mathf.Tan(Mathf.Abs(tiltx)) * Mathf.Sign(tiltx) - drag * ux * ux * Mathf.Sign(ux);
        float fy = mass * 9.81f * Mathf.Tan(Mathf.Abs(tilty)) * Mathf.Sign(tilty) - drag * uy * uy * Mathf.Sign(uy);
        rb.AddForce(fx * Responsiveness, 0, fy * Responsiveness);
    }
}
