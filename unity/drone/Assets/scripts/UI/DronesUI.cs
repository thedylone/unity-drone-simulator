using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DronesUI : MonoBehaviour
{
    public InputField DistanceField;
    public InputField DroneASpeedField;
    public InputField DroneBSpeedField;
    public Dropdown DroneBDropdown;
    public Text OutputText;
    List<GameObject> droneModels;
    void Start()
    {
        distanceChange(DistanceField.text);
        droneAChange(DroneASpeedField.text);
        droneBChange(DroneBSpeedField.text);
        loadResources();
        updateDropdownOptions();
        DistanceField.onEndEdit.AddListener(distanceChange);
        DroneASpeedField.onEndEdit.AddListener(droneAChange);
        DroneBSpeedField.onEndEdit.AddListener(droneBChange);
        DroneBDropdown.onValueChanged.AddListener(dropdownChange);
        DroneBDropdown.value = Mathf.Min(PlayerPrefs.GetInt(DroneBDropdown.name), DroneBDropdown.options.Count);
        dropdownChange(DroneBDropdown.value);
    }

    void loadResources()
    {
        droneModels = new List<GameObject>(Resources.LoadAll<GameObject>("Drone Models"));
    }
    void distanceChange(string distance)
    {
        Settings.DroneDistance = float.Parse(distance);
    }
    void droneAChange(string speed)
    {
        Settings.DroneASpeed = float.Parse(speed);
    }
    void droneBChange(string speed)
    {
        Settings.DroneBSpeed = float.Parse(speed);
    }
    void updateDropdownOptions()
    {
        DroneBDropdown.GetComponent<Dropdown>().ClearOptions();
        DroneBDropdown.GetComponent<Dropdown>().AddOptions(new List<string>(droneModels.Select(x => x.name)));
    }
    void dropdownChange(int index)
    {
        Settings.DroneBModel = droneModels[index];
        OutputText.text = "Drone model selected:\n" + Settings.DroneBModel.name;
    }
}
