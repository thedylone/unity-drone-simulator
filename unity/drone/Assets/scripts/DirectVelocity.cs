using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectVelocity : MonoBehaviour
{
    public float MaxTiltDeg = 25f;
    public bool EnableTilt;
    public float TiltSpeed = 1;
    GameObject drone;
    Rigidbody rb;
    float mass;
    float drag;

    // void Update()
    // {
    //     Convert(vx, vy);
    // }

    public void Convert(float vx, float vy)
    {
        // takes vx vy input from -1 to 1 and converts it to the drone's velocity
        float MaxSpeed = GetComponent<DroneController>().MaxSpeed;
        drone = GetComponent<DroneController>().Drone;
        rb = drone.GetComponent<Rigidbody>();
        mass = rb.mass;
        // rb.drag = 0;
        drag = mass * 9.81f * Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) / (MaxSpeed * MaxSpeed);
        float ux = rb.velocity.x;
        float uy = rb.velocity.z;
        // prevent vx and vy from exceeding -1 to 1
        vx = Mathf.Min(Mathf.Abs(vx), 1) * Mathf.Sign(vx);
        vy = Mathf.Min(Mathf.Abs(vy), 1) * Mathf.Sign(vy);

        if (EnableTilt)
        {
            // float trueTiltx = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vx * vx) * Mathf.Sign(vx);
            float trueTiltx = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vx * vx);
            // float trueTilty = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vy * vy) * Mathf.Sign(vy);
            float trueTilty = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vy * vy);

            float currentTiltx = transform.rotation.eulerAngles.z > 180 ? transform.rotation.eulerAngles.z - 360 : transform.rotation.eulerAngles.z;
            currentTiltx *= -1;
            float currentTilty = transform.rotation.eulerAngles.x > 180 ? transform.rotation.eulerAngles.x - 360 : transform.rotation.eulerAngles.x;

            float dx = vx * MaxSpeed + (vx * MaxSpeed - ux) * 1;
            float dy = vy * MaxSpeed + (vy * MaxSpeed - uy) * 1;

            float tiltx = Mathf.Min(MaxTiltDeg, Mathf.Atan(dx * dx * drag / (9.81f * mass)) * 180 / Mathf.PI) * Mathf.Sign(dx);
            float tilty = Mathf.Min(MaxTiltDeg, Mathf.Atan(dy * dy * drag / (9.81f * mass)) * 180 / Mathf.PI) * Mathf.Sign(dy);

            tiltx = tiltx < currentTiltx ? Mathf.Max(tiltx, currentTiltx - TiltSpeed * Time.deltaTime) : Mathf.Min(tiltx, currentTiltx + TiltSpeed * Time.deltaTime);
            tilty = tilty < currentTilty ? Mathf.Max(tilty, currentTilty - TiltSpeed * Time.deltaTime) : Mathf.Min(tilty, currentTilty + TiltSpeed * Time.deltaTime);

            rb.rotation = Quaternion.Euler(new Vector3(tilty, 0, -tiltx));
        }


        // float fx = mass * 9.81f * Mathf.Tan(currentTiltx * Mathf.PI / 180) - drag * ux * ux * Mathf.Sign(ux);
        // float fy = mass * 9.81f * Mathf.Tan(currentTilty * Mathf.PI / 180) - drag * uy * uy * Mathf.Sign(uy);
        // rb.AddForce(fx, 0, fy);
        rb.velocity = new Vector3(vx * MaxSpeed, 0, vy * MaxSpeed);
    }
}
