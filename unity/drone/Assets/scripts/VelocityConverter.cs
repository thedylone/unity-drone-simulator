using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]

public class VelocityConverter : MonoBehaviour
{
    public static void Convert(Rigidbody rb, float vx, float vy, float maxSpeed, float maxTiltDeg, float tiltSpeed)
    {
        // takes vx vy input from -1 to 1 and add force to the drone to reach target velocity
        // rb = GetComponent<DroneController>().Drone.GetComponent<Rigidbody>();
        // float MaxSpeed = GetComponent<DroneController>().MaxSpeed;
        float mass = rb.mass;
        rb.drag = 0;
        float dragCoefficient = mass * 9.81f * Mathf.Tan(maxTiltDeg * Mathf.PI / 180) / maxSpeed;
        Debug.Log(dragCoefficient);
        float ux = rb.velocity.x;
        float uy = rb.velocity.z;
        // // prevent vx and vy from exceeding -1 to 1
        // vx = Mathf.Clamp(vx, -1, 1);
        // vy = Mathf.Clamp(vy, -1, 1);

        // float trueTiltx = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vx * vx) * Mathf.Sign(vx);
        float trueTiltx = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.Deg2Rad) * vx);
        // float trueTilty = Mathf.Atan(Mathf.Tan(MaxTiltDeg * Mathf.PI / 180) * vy * vy) * Mathf.Sign(vy);
        float trueTilty = Mathf.Atan(Mathf.Tan(maxTiltDeg * Mathf.Deg2Rad) * vy);

        float currentTiltx = rb.rotation.eulerAngles.z > 180 ? rb.rotation.eulerAngles.z - 360 : rb.rotation.eulerAngles.z;
        currentTiltx *= -1;
        float currentTilty = rb.rotation.eulerAngles.x > 180 ? rb.rotation.eulerAngles.x - 360 : rb.rotation.eulerAngles.x;

        Debug.Log($"{currentTiltx},{currentTilty}");

        float dx = vx * maxSpeed + (vx * maxSpeed - ux) * 10;
        float dy = vy * maxSpeed + (vy * maxSpeed - uy) * 10;

        Debug.Log($"{dx},{dy}");

        float tiltx = Mathf.Clamp(Mathf.Atan(dx * dragCoefficient / (9.81f * mass)) * Mathf.Rad2Deg, -maxTiltDeg, maxTiltDeg);
        float tilty = Mathf.Clamp(Mathf.Atan(dy * dragCoefficient / (9.81f * mass)) * Mathf.Rad2Deg, -maxTiltDeg, maxTiltDeg);

        // tiltx = tiltx < currentTiltx ? Mathf.Max(tiltx, currentTiltx - tiltSpeed * Time.deltaTime) : Mathf.Min(tiltx, currentTiltx + tiltSpeed * Time.deltaTime);
        // tilty = tilty < currentTilty ? Mathf.Max(tilty, currentTilty - tiltSpeed * Time.deltaTime) : Mathf.Min(tilty, currentTilty + tiltSpeed * Time.deltaTime);

        rb.rotation = Quaternion.Euler(new Vector3(tilty, 0, -tiltx));

        float fx = mass * 9.81f * Mathf.Tan(currentTiltx * Mathf.Deg2Rad) - dragCoefficient * ux;
        Debug.Log(Mathf.Tan(currentTiltx * Mathf.Deg2Rad));
        float fy = mass * 9.81f * Mathf.Tan(currentTilty * Mathf.Deg2Rad) - dragCoefficient * uy;
        Debug.Log($"{fx},{fy}");
        rb.AddForce(fx, 0, fy);
    }
}
