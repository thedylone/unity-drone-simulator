using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    public GameObject DroneController;
    public InputField PosX;
    public InputField PosY;
    public InputField PosZ;
    public InputField Speed;
    
    void Start()
    {
        updateInputs();
        transformPosition();
    }
    public void updateInputs()
    {
        GameObject Drone = DroneController.GetComponent<DroneController>().Drone;
        Vector3 initialPosition = Drone.transform.localPosition;
        PosX.text = initialPosition.x.ToString();
        PosY.text = initialPosition.y.ToString();
        PosZ.text = initialPosition.z.ToString();
        Speed.text = DroneController.GetComponent<DroneController>()._MaxSpeed.ToString();
    }
    public void transformPosition()
    {
        GameObject Drone = DroneController.GetComponent<DroneController>().Drone;
        Drone.transform.localPosition = new Vector3(float.Parse(PosX.text), float.Parse(PosY.text), float.Parse(PosZ.text));
    }
}
