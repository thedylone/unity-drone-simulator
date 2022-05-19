using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectVelocity : MonoBehaviour
{
    public float maxTiltDeg = 25f;
    public bool enableTilt;
    public float tiltSpeed = 1;
    GameObject drone;
    Rigidbody rb;
    float mass;
    float drag;

    // void Update()
    // {
    //     Convert(vx, vy);
    // }

    public static void Convert(Rigidbody rb, float vx, float vy, float maxSpeed, float maxTiltDeg, float tiltSpeed, bool enableTilt)
    {
        // takes vx vy input from -1 to 1 and converts it to the drone's velocity
        // float maxSpeed = GetComponent<DroneController>().maxSpeed;
        // drone = GetComponent<DroneController>().Drone;
        // rb = drone.GetComponent<Rigidbody>();
        float mass = rb.mass;
        // rb.drag = 0;
        float drag = mass * 9.81f * Mathf.Tan(maxTiltDeg * Mathf.PI / 180) / (maxSpeed * maxSpeed);
        float ux = rb.velocity.x;
        float uy = rb.velocity.z;
        // prevent vx and vy from exceeding -1 to 1
        vx = Mathf.Min(Mathf.Abs(vx), 1) * Mathf.Sign(vx);
        vy = Mathf.Min(Mathf.Abs(vy), 1) * Mathf.Sign(vy);

        if (enableTilt)
        {
            // float trueTiltx = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.PI / 180) * vx * vx) * Mathf.Sign(vx);
            float trueTiltx = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.PI / 180) * vx * vx);
            // float trueTilty = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.PI / 180) * vy * vy) * Mathf.Sign(vy);
            float trueTilty = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.PI / 180) * vy * vy);

            float currentTiltx = rb.rotation.eulerAngles.z > 180 ? rb.rotation.eulerAngles.z - 360 : rb.rotation.eulerAngles.z;
            currentTiltx *= -1;
            float currentTilty = rb.rotation.eulerAngles.x > 180 ? rb.rotation.eulerAngles.x - 360 : rb.rotation.eulerAngles.x;

            float dx = vx * maxSpeed + (vx * maxSpeed - ux) * 1;
            float dy = vy * maxSpeed + (vy * maxSpeed - uy) * 1;

            float tiltx = Mathf.Min(maxTiltDeg, Mathf.Atan(dx * dx * drag / (9.81f * mass)) * 180 / Mathf.PI) * Mathf.Sign(dx);
            float tilty = Mathf.Min(maxTiltDeg, Mathf.Atan(dy * dy * drag / (9.81f * mass)) * 180 / Mathf.PI) * Mathf.Sign(dy);

            tiltx = tiltx < currentTiltx ? Mathf.Max(tiltx, currentTiltx - tiltSpeed * Time.deltaTime) : Mathf.Min(tiltx, currentTiltx + tiltSpeed * Time.deltaTime);
            tilty = tilty < currentTilty ? Mathf.Max(tilty, currentTilty - tiltSpeed * Time.deltaTime) : Mathf.Min(tilty, currentTilty + tiltSpeed * Time.deltaTime);

            rb.rotation = Quaternion.Euler(new Vector3(tilty, 0, -tiltx));
        }


        // float fx = mass * 9.81f * Mathf.Tan(currentTiltx * Mathf.PI / 180) - drag * ux * ux * Mathf.Sign(ux);
        // float fy = mass * 9.81f * Mathf.Tan(currentTilty * Mathf.PI / 180) - drag * uy * uy * Mathf.Sign(uy);
        // rb.AddForce(fx, 0, fy);
        rb.velocity = new Vector3(vx * maxSpeed, 0, vy * maxSpeed);
    }
}
