using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]

public class VelocityConverter : MonoBehaviour
{
    public enum ConvertMode { Direct, Velocity, ControllerManager };
    public ConvertMode UseMode;
    public float Vx;
    public float Vy;
    Rigidbody rb
    {
        get { return GetComponent<DroneController>().Drone.GetComponent<Rigidbody>(); }
    }
    float maxSpeed
    {
        get { return GetComponent<DroneController>().MaxSpeed; }
    }
    ControllerManager cm
    {
        get { return GetComponent<ControllerManager>(); }
    }
    float maxTiltDeg = 25;
    float tiltSpeed = 1;
    public static void Convert(Rigidbody rb, float vx, float vy, float maxSpeed, float maxTiltDeg, float tiltSpeed)
    {
        // takes vx vy input from -1 to 1 and add force to the drone to reach target velocity
        // rb = GetComponent<DroneController>().Drone.GetComponent<Rigidbody>();
        // float MaxSpeed = GetComponent<DroneController>().MaxSpeed;
        float mass = rb.mass;
        rb.drag = 0;
        // 25 is the true max speed of the drone
        float dragCoefficient = mass * 9.81f * Mathf.Tan(maxTiltDeg * Mathf.PI / 180) / 25;
        // Debug.Log(dragCoefficient);
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

        // Debug.Log($"{currentTiltx},{currentTilty}");
        // 10 is an arbitrary constant to create a larger dx/dy 
        float dx = vx * maxSpeed + (vx * maxSpeed - ux) * 10;
        float dy = vy * maxSpeed + (vy * maxSpeed - uy) * 10;

        // Debug.Log($"{dx},{dy}");

        float tiltx = Mathf.Clamp(Mathf.Atan(dx * dragCoefficient / (9.81f * mass)) * Mathf.Rad2Deg, -maxTiltDeg, maxTiltDeg);
        float tilty = Mathf.Clamp(Mathf.Atan(dy * dragCoefficient / (9.81f * mass)) * Mathf.Rad2Deg, -maxTiltDeg, maxTiltDeg);

        // tiltx = tiltx < currentTiltx ? Mathf.Max(tiltx, currentTiltx - tiltSpeed * Time.deltaTime) : Mathf.Min(tiltx, currentTiltx + tiltSpeed * Time.deltaTime);
        // tilty = tilty < currentTilty ? Mathf.Max(tilty, currentTilty - tiltSpeed * Time.deltaTime) : Mathf.Min(tilty, currentTilty + tiltSpeed * Time.deltaTime);

        rb.rotation = Quaternion.Euler(new Vector3(tilty, 0, -tiltx));

        float fx = mass * 9.81f * Mathf.Tan(currentTiltx * Mathf.Deg2Rad) - dragCoefficient * ux;
        // Debug.Log(Mathf.Tan(currentTiltx * Mathf.Deg2Rad));
        float fy = mass * 9.81f * Mathf.Tan(currentTilty * Mathf.Deg2Rad) - dragCoefficient * uy;
        // Debug.Log($"{fx},{fy}");
        rb.AddForce(fx, 0, fy);
    }

    public static void DirectConvert(Rigidbody rb, float vx, float vy, float maxSpeed)
    {
        rb.velocity = new Vector3(vx * maxSpeed, 0, vy * maxSpeed);
    }

    static float prevErrorX = 0;
    static float prevErrorY = 0;
    static float errorSumX = 0;
    static float errorSumY = 0;
    public static void ControllerConvert(ControllerManager cm, Rigidbody rb, float vx, float vy, float maxSpeed)
    {
        float gain_P = 4e-2f;
        float gain_I = 4e-2f;
        float gain_D = 1e-4f;
        float errorMax = 20f;

        // cm.rb = rb;
        // rb.inertiaTensor = new Vector3(12e-4f, 15e-4f, 10e-4f);

        float errorX = vx * maxSpeed - rb.velocity.x;
        float errorY = vy * maxSpeed - rb.velocity.z;

        float dx = gain_P * errorX;
        float dy = gain_P * errorY;

        errorSumX += Time.fixedDeltaTime * dx;
        errorSumY += Time.fixedDeltaTime * dy;

        errorSumX = Mathf.Clamp(errorSumX, -errorMax, errorMax);
        errorSumY = Mathf.Clamp(errorSumY, -errorMax, errorMax);

        dx += gain_I * errorSumX;
        dy += gain_I * errorSumY;

        float dtErrorX = (errorX - prevErrorX) / Time.fixedDeltaTime;
        float dtErrorY = (errorY - prevErrorY) / Time.fixedDeltaTime;

        prevErrorX = errorX;
        prevErrorY = errorY;

        dx += gain_D * dtErrorX;
        dy += gain_D * dtErrorY;

        // cm.AileronInput = Mathf.Clamp(dx, -1, 1);
        // cm.ElevatorInput = Mathf.Clamp(dy, -1, 1);

        cm.AileronInput = dx;
        cm.ElevatorInput = dy;
    }

    public void FixedUpdate()
    {
        switch (UseMode)
        {
            case ConvertMode.Direct:
                DirectConvert(rb, Vx, Vy, maxSpeed);
                break;
            case ConvertMode.Velocity:
                Convert(rb, Vx, Vy, maxSpeed, maxTiltDeg, tiltSpeed);
                break;
            case ConvertMode.ControllerManager:
                cm.rb = rb;
                rb.inertiaTensor = new Vector3(12e-4f, 15e-4f, 10e-4f);
                rb.useGravity = true;
                rb.mass = 0.4f;
                rb.drag = 0.35f;
                ControllerConvert(cm, rb, Vx, Vy, maxSpeed);
                break;
        }
        // Convert(rb, Vx, Vy, maxSpeed, maxTiltDeg, tiltSpeed);
        // DirectConvert(rb, Vx, Vy, maxSpeed);
        // ControllerConvert(cm, rb, Vx, Vy, maxSpeed);
    }

    public void SetVelocities(float vx, float vy)
    {
        Vx = vx;
        Vy = vy;
    }
}
