using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleUI : MonoBehaviour
{
    public DroneController Target;
    public InputField RadiusField;
    public InputField SpeedField;
    public Toggle ClockwiseToggle;
    public Toggle EnableToggle;
    public Toggle DisableToggle;

    public bool IsOn;
    public bool Clockwise;
    void Start()
    {
        ClockwiseToggle.onValueChanged.AddListener(clockwiseChange);
        EnableToggle.onValueChanged.AddListener(enableChange);
    }

    void Update()
    {
        if (IsOn) orbit(Target.GetComponent<DroneController>(), int.Parse(RadiusField.text), int.Parse(SpeedField.text), Clockwise);
    }
    void clockwiseChange(bool isOn)
    {
        Clockwise = isOn;
    }
    void enableChange(bool isOn)
    {
        IsOn = isOn;
        Target.GetComponent<KeyboardController>().enabled = !isOn;
    }
    float timer = 0;
    void orbit(DroneController target, int radius, float speed, bool clockwise)
    {
        if (!target.waypointManager.WaypointLoadStarted)
        {
            // target.GetComponent<KeyboardController>().converter.Convert();
            // float x = Mathf.Cos(timer) * radius * (clockwise ? -1 : 1);
            // float y = Mathf.Sin(timer) * radius;
            // target.Drone.transform.localPosition = new Vector3(x, target.Drone.transform.localPosition.y, y);
            float vx = speed * Mathf.Sin(timer * speed / radius) * (clockwise ? 1 : -1) / target.MaxSpeed;
            float vy = speed * Mathf.Cos(timer * speed / radius) / target.MaxSpeed;
            target.GetComponent<VelocityConverter>().SetVelocities(vx, vy);
            timer += Time.deltaTime;
        }
        else
        {
            IsOn = false;
            EnableToggle.isOn = false;
            DisableToggle.isOn = true;
        }
    }
}