using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class VelocityConverter : MonoBehaviour
{
    public float MaxSpeed = 15f;
    public float MaxTiltDeg = 25f;
    public float tiltSpeed = 1;
    [Range(-1, 1)]
    public float vx = 0.5f;
    [Range(-1, 1)]
    public float vy = 0.5f;

    private Rigidbody rb;
    private float mass;
    private float drag;

    void Update()
    {
        Convert(vx, vy);
    }

    public void Convert(float vx, float vy)
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        rb.drag = 0;
        drag = mass * 9.81f * Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) / (MaxSpeed * MaxSpeed);
        float ux = rb.velocity.x;
        float uy = rb.velocity.z;
        // prevent vx and vy from exceeding -1 to 1
        vx = Mathf.Min(Mathf.Abs(vx), 1) * Mathf.Sign(vx);
        vy = Mathf.Min(Mathf.Abs(vy), 1) * Mathf.Sign(vy);

        // float trueTiltx = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vx * vx) * Mathf.Sign(vx);
        float trueTiltx = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vx * vx);
        // float trueTilty = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vy * vy) * Mathf.Sign(vy);
        float trueTilty = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vy * vy);

        float currentTiltx = transform.rotation.eulerAngles.z;
        float currentTilty = transform.rotation.eulerAngles.x;

        float dx = vx * MaxSpeed - ux;
        float dy = vy * MaxSpeed - uy;

        float tiltx = Mathf.Min(MaxTiltDeg, Mathf.Abs(Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * dx * dx) * 180 / Mathf.PI)) * Mathf.Sign(dx);
        float tilty = Mathf.Min(MaxTiltDeg, Mathf.Abs(Mathf.Atan(dy * dy * drag / (9.81f * mass)) * 180 / Mathf.PI)) * Mathf.Sign(dy);
        
        // rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
        // transform.Rotate(tilty * 180 / Mathf.PI, 0, tiltx * 180 / Mathf.PI, Space.Self);
        rb.rotation = Quaternion.Euler(new Vector3(tilty, 0, -tiltx));
        
        float fx = mass * 9.81f * Mathf.Tan(-currentTiltx * Mathf.PI / 180) - drag * ux * ux * Mathf.Sign(ux);
        float fy = mass * 9.81f * Mathf.Tan(currentTilty * Mathf.PI / 180) - drag * uy * uy * Mathf.Sign(uy);
        rb.AddForce(fx, 0, fy);
    }
}