using UnityEngine;
using System.Collections;
// using Unity.MLAgents;
// using Unity.MLAgents.Sensors;
// using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class ControllerManager : MonoBehaviour
{
    public enum ControlMode { Stabilized, Manual };

    public ControlMode anglesControl = ControlMode.Stabilized;
    public float tiltAngle = 20;
    public float rollSensitivity = 100;
    public float pitchSensitivity = 100;
    public float yawSensitivity = 100;
    public float tiltAngleKp = 4;
    public float yawAngleKp = 2;
    public float tiltRateKp = 10;
    public float yawRateKp = 10;

    public ControlMode heightControl = ControlMode.Stabilized;
    public float verticalSpeedSensitivity = 3;
    public float heightSensitivity = 3000;
    public float verticalSpeedKp = 2;

    public bool squareMapping = true;
    public bool invertAxisT = false;
    public bool invertAxisR = false;
    public bool invertAxisE = false;
    public bool invertAxisA = false;

    public float ElevatorInput = 0;
    public float AileronInput = 0;

    private float rollRef;
    private float pitchRef;
    private float yawRef;
    private Vector3 angularRatesRef;
    private float verticalSpeedRef;
    public Rigidbody rb;
    private Vector3 acceleration;
    private float commandsMultiplier;
    private float lastUserRudderInput;
    private float heightFeedforward = 8200;

    private PropellersController propellersController;

    Vector4 controlSignal;
    //BoundingBox boundingBox;
    //public GameObject targetDrone;
    //List<float> throttleMemory;
    //List<float> yawMemory;
    //bool homing = false;

    void Start()
    {
        propellersController = GetComponent<PropellersController>();
        // rb = GetComponent<Rigidbody>();

        rb.inertiaTensor = new Vector3(12e-4f, 15e-4f, 10e-4f);

        // LoadPlayerPrefs ();
        resetYawRef();
        Dbg.Trace("phi, theta, psi, phi_ref, theta_ref, psi_ref, p, q, r, p_ref, q_ref, r_ref, torque_x, torque_y, torque_z, vz, vz_ref, thrust_Z");
        // heightControl = ControlMode.Stabilized;
        // anglesControl = ControlMode.Stabilized;
        //controlSignal = Vector4.zero;
        //boundingBox = GetComponent<BoundingBox>();
        //yawMemory = new List<float>();
        //throttleMemory = new List<float>();
    }
    /*
    private void Update()
    {
		if (!homing)
        {
			// if not homing but target in cv, adjust
			if (boundingBox.DrawBoundingBox(targetDrone, 15, out Vector2 boundingBoxCenter))
			{
				if (boundingBoxCenter.x == 0)
					controlSignal.x = 0f;
				else
					controlSignal.x = Mathf.Clamp(boundingBoxCenter.x * 10, -1, 1);

				if (boundingBoxCenter.y == 0)
					controlSignal.w = .68f;
				else
					controlSignal.w = .68f + boundingBoxCenter.y * .32f;

				yawMemory.Add(controlSignal.x);
				if (yawMemory.Count == 181)
					yawMemory.RemoveAt(0);
				throttleMemory.Add(controlSignal.w);
				if (throttleMemory.Count == 181)
					throttleMemory.RemoveAt(0);


				bool isAdjusting = false;
				foreach (float yawValue in yawMemory)
				{
					if (Mathf.Abs(yawValue) > 0.02f)
					{
						isAdjusting = true;
						break;
					}
				}
				foreach (float throttleValue in throttleMemory)
				{
					if (Mathf.Abs(0.68f - throttleValue) > 0.02f)
					{
						isAdjusting = true;
						break;
					}
				}
				if (yawMemory.Count < 180 && throttleMemory.Count < 180)
					isAdjusting = true;
				if (!isAdjusting)
				{
					homing = true;
				}
			}
			// if not homing and target not in cv, scan
            else
            {
				// scan
            }
		}
		// if homing
        else
        {
			// set homing back to false if cv no longer spots target
			if (!boundingBox.DrawBoundingBox(targetDrone, 15, out Vector2 boundingBoxCenter))
            {
				homing = false;
            }

		}
			
	}*/
    void FixedUpdate()
    {
        float heightCommandCompensated = 0;
        float heightFeedforwardCompensated = 0;
        Vector3 anglesCommand = Vector3.zero;

        switch (heightControl)
        {
            case ControlMode.Stabilized:
                heightFeedforwardCompensated = applyAngleCompensation(heightFeedforward);
                heightCommandCompensated = applyAngleCompensation(controlVerticalSpeed());
                break;
            case ControlMode.Manual:
                heightFeedforwardCompensated = controlThrottle(controlSignal);
                //Debug.Log(heightFeedforwardCompensated);
                heightCommandCompensated = 0;
                break;
        }
        switch (anglesControl)
        {
            case ControlMode.Stabilized:
                anglesCommand = controlAngles(0);
                break;
            case ControlMode.Manual:
                anglesCommand = controlRates(controlSignal);
                break;
        }/*
		if (homing)
		{
			anglesCommand = controlAngles(1f);
			heightFeedforwardCompensated = applyAngleCompensation(heightFeedforward);
			heightCommandCompensated = applyAngleCompensation(controlVerticalSpeed());
		}*/

        propellersController.MixRPM(heightFeedforwardCompensated, heightCommandCompensated, anglesCommand.x, anglesCommand.y, anglesCommand.z);
        //		Dbg.Trace (rb.angularVelocity.ToString());
        Vector3 f = propellersController.getTotalForce();
        Vector3 t = propellersController.getTotalTorque();
        rb.AddRelativeForce(f);
        rb.AddRelativeTorque(t);
        /*
		Dbg.Trace (string.Format("{0:0.0000000}, {1:0.0000000}, {2:0.0000000}, {3:0.0000000}, {4:0.0000000}, {5:0.0000000}, " +
		                         "{6:0.0000000}, {7:0.0000000}, {8:0.0000000}, {9:0.0000000}, {10:0.0000000}, {11:0.0000000}, " + 
		                         "{12:0.0000000}, {13:0.0000000}, {14:0.0000000}, {15:0.0000000}, {16:0.0000000}, {17:0.0000000}", 
		                         transform.eulerAngles.z, transform.eulerAngles.x, transform.eulerAngles.y, rollRef, pitchRef, yawRef,
		                         -rb.angularVelocity.z, -rb.angularVelocity.x, rb.angularVelocity.y,
		                         angularRatesRef.x, angularRatesRef.y, angularRatesRef.z, -t.z, -t.x, t.y,
		                         rb.velocity.y, verticalSpeedRef, f.y));*/
        //acceleration = transform.rotation * f / rb.mass - 9.81f*Vector3.up;
    }

    // for manual height control
    float controlThrottle(Vector4 controlSignal)
    {
        float userInput = Input.GetAxis("Drone Throttle") * (invertAxisT ? -1 : 1);
        // float userInput = controlSignal.w * (invertAxisT ? -1 : 1);
        float throttle = heightSensitivity * userInput;
        return throttle;
    }
    // for stabilised height control
    float controlVerticalSpeed()
    {
        float userInput = Input.GetAxis("Drone Throttle") * (invertAxisT ? -1 : 1);
        verticalSpeedRef = verticalSpeedSensitivity * userInput;
        return heightLoop(verticalSpeedRef);
    }
    // for stabilised height control
    float heightLoop(float verticalSpeedRef)
    {
        float verticalSpeed = rb.velocity.y;
        float Cz = 0.4f;
        return verticalSpeedKp * (verticalSpeedRef - verticalSpeed) + Cz * verticalSpeed;
    }
    // for stabilised height control
    float applyAngleCompensation(float heightCommand)
    {
        float coscos = Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
        if (coscos > 0)
        {
            return heightCommand / Mathf.Sqrt(coscos);
        }
        else
        {
            return 0;
        }
    }
    // for manual angle control
    Vector3 controlRates(Vector4 controlSignal)
    {
        // float userRudderInput = Input.GetAxis ("Drone Rudder") * (invertAxisR ? -1 : 1);
        // float userAileronInput = Input.GetAxis ("Drone Aileron") * (invertAxisA ? -1 : 1);
        // float userElevatorInput = Input.GetAxis ("Drone Elevator") * (invertAxisE ? -1 : 1);

        float userRudderInput = 0;
        float userAileronInput = AileronInput * (invertAxisA ? -1 : 1);
        float userElevatorInput = ElevatorInput * (invertAxisE ? -1 : 1);

        // float userRudderInput = controlSignal.x * (invertAxisR ? -1 : 1);
        // float userAileronInput = controlSignal.y * (invertAxisA ? -1 : 1);
        // float userElevatorInput = controlSignal.z * (invertAxisE ? -1 : 1);

        // float boost = Input.GetAxis("Drone Boost");
		float boost = 0;
        commandsMultiplier = 1 + boost;

        if (squareMapping)
        {
            userRudderInput *= Mathf.Abs(userRudderInput);
            userAileronInput *= Mathf.Abs(userAileronInput);
            userElevatorInput *= Mathf.Abs(userElevatorInput);
        }
        userRudderInput *= commandsMultiplier;
        userAileronInput *= commandsMultiplier;
        userElevatorInput *= commandsMultiplier;

        float rollRateRef = rollSensitivity * Mathf.Deg2Rad * userAileronInput;
        float pitchRateRef = -pitchSensitivity * Mathf.Deg2Rad * userElevatorInput;
        float yawRateRef = yawSensitivity * Mathf.Deg2Rad * userRudderInput;
        angularRatesRef = new Vector3(rollRateRef, pitchRateRef, yawRateRef);

        return rateLoop(angularRatesRef);
    }
    // for stabilised angle control
    Vector3 controlAngles(float a)
    {
        // float userRudderInput = Input.GetAxis("Drone Rudder") * (invertAxisR ? -1 : 1);
        // float userAileronInput = Input.GetAxis("Drone Aileron") * (invertAxisA ? -1 : 1);
        // float userElevatorInput = Input.GetAxis("Drone Elevator") * (invertAxisE ? -1 : 1);

		float userRudderInput = 0;
        float userAileronInput = AileronInput * (invertAxisA ? -1 : 1);
        float userElevatorInput = ElevatorInput * (invertAxisE ? -1 : 1);

        // float boost = Input.GetAxis("Drone Boost");
		float boost = 0;
        commandsMultiplier = 1 + boost;

        if (squareMapping)
        {
            userRudderInput *= Mathf.Abs(userRudderInput);
            userAileronInput *= Mathf.Abs(userAileronInput);
            userElevatorInput *= Mathf.Abs(userElevatorInput);
        }
        userRudderInput *= commandsMultiplier;
        userAileronInput *= commandsMultiplier;
        userElevatorInput *= commandsMultiplier;

        rollRef = tiltAngle * userAileronInput;
        pitchRef = -tiltAngle * userElevatorInput;
        yawRef += yawSensitivity * userRudderInput * Time.deltaTime;
        if ((lastUserRudderInput != 0) && (userRudderInput == 0))
        {
            yawRef += angularRatesRef.z * 1 / yawRateKp;
        }
        lastUserRudderInput = userRudderInput;

        return quaternionLoop(Quaternion.Euler(-pitchRef, yawRef, -rollRef));
    }

    Vector3 quaternionLoop(Quaternion quaternionRef)
    {
        Quaternion quaternionEst = transform.rotation;
        Quaternion quaternionError = Quaternion.Inverse(quaternionEst) * quaternionRef;
        Vector3 axis = Vector3.zero; float angle = 0;
        quaternionError.ToAngleAxis(out angle, out axis);
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 180;
        if (double.IsInfinity(axis.magnitude))
            axis = Vector3.zero;
        angularRatesRef = angle * Mathf.Deg2Rad * (new Vector3(-axis.z, -axis.x, axis.y));
        angularRatesRef.x *= tiltAngleKp;
        angularRatesRef.y *= tiltAngleKp;
        angularRatesRef.z *= yawAngleKp;
        return rateLoop(angularRatesRef);
    }

    Vector3 rateLoop(Vector3 angularRatesRef)
    {
        Vector3 omega = transform.InverseTransformDirection(rb.angularVelocity);
        float rollCommand = tiltRateKp * (angularRatesRef.x + omega.z);
        float pitchCommand = tiltRateKp * (angularRatesRef.y + omega.x);
        float yawCommand = yawRateKp * (angularRatesRef.z - omega.y);
        return new Vector3(rollCommand, pitchCommand, yawCommand);
    }

    public void resetYawRef()
    {
        yawRef = transform.eulerAngles.y;
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("tiltSensitivity"))
        {
            rollSensitivity = PlayerPrefs.GetFloat("tiltSensitivity");
            pitchSensitivity = PlayerPrefs.GetFloat("tiltSensitivity");
        }
        if (PlayerPrefs.HasKey("yawSensitivity"))
        {
            yawSensitivity = PlayerPrefs.GetFloat("yawSensitivity");
        }
        if (PlayerPrefs.HasKey("heightSensitivity"))
        {
            heightSensitivity = PlayerPrefs.GetFloat("heightSensitivity");
        }
        if (PlayerPrefs.HasKey("squareMapping"))
        {
            squareMapping = (PlayerPrefs.GetInt("squareMapping") > 0);
        }
        if (PlayerPrefs.HasKey("invertAxisT"))
        {
            invertAxisT = (PlayerPrefs.GetInt("invertAxisT") > 0);
        }
        if (PlayerPrefs.HasKey("invertAxisR"))
        {
            invertAxisR = (PlayerPrefs.GetInt("invertAxisR") > 0);
        }
        if (PlayerPrefs.HasKey("invertAxisE"))
        {
            invertAxisE = (PlayerPrefs.GetInt("invertAxisE") > 0);
        }
        if (PlayerPrefs.HasKey("invertAxisA"))
        {
            invertAxisA = (PlayerPrefs.GetInt("invertAxisA") > 0);
        }
        if (PlayerPrefs.HasKey("tiltRateKp"))
        {
            tiltRateKp = PlayerPrefs.GetFloat("tiltRateKp");
        }
        if (PlayerPrefs.HasKey("yawRateKp"))
        {
            yawRateKp = PlayerPrefs.GetFloat("yawRateKp");
        }
        if (PlayerPrefs.HasKey("enableStabilization"))
        {
            bool enableStabilization = (PlayerPrefs.GetInt("enableStabilization") > 0);
            if (enableStabilization)
            {
                anglesControl = ControlMode.Stabilized;
                heightControl = ControlMode.Stabilized;
            }
            else
            {
                anglesControl = ControlMode.Manual;
                heightControl = ControlMode.Manual;
            }
        }
        if (PlayerPrefs.HasKey("tiltAngle"))
        {
            tiltAngle = PlayerPrefs.GetFloat("tiltAngle");
        }
        if (PlayerPrefs.HasKey("verticalSpeed"))
        {
            verticalSpeedSensitivity = PlayerPrefs.GetFloat("verticalSpeed");
        }
        if (PlayerPrefs.HasKey("tiltAngleKp"))
        {
            tiltAngleKp = PlayerPrefs.GetFloat("tiltAngleKp");
        }
        if (PlayerPrefs.HasKey("yawAngleKp"))
        {
            yawAngleKp = PlayerPrefs.GetFloat("yawAngleKp");
        }
        if (PlayerPrefs.HasKey("verticalSpeedKp"))
        {
            verticalSpeedKp = PlayerPrefs.GetFloat("verticalSpeedKp");
        }
    }

    public Vector3 getAcceleration()
    {
        return acceleration;
    }
}
