using UnityEngine;
using System.Collections;

public class ResetDrone : MonoBehaviour {
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Restart")) {
			Restart();
		}
	}

	public void Restart() {
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		// GetComponent<StabilisedAIController> ().resetYawRef();
	}
}
