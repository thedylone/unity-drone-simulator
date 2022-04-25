using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DronesUI : MonoBehaviour
{
    public InputField DroneASpeedField;
    public InputField DroneBSpeedField;
    public Dropdown DroneBDropdown;
    List<GameObject> droneModels;
    void Start()
    {
        droneAChange(DroneASpeedField.text);
        droneBChange(DroneBSpeedField.text);
        loadResources();
        updateDropdownOptions();
        DroneASpeedField.onEndEdit.AddListener(droneAChange);
        DroneBSpeedField.onEndEdit.AddListener(droneBChange);
        DroneBDropdown.onValueChanged.AddListener(dropdownChange);
        dropdownChange(DroneBDropdown.value);
    }

    void loadResources()
    {
        droneModels = new List<GameObject>(Resources.LoadAll<GameObject>("Drone Models"));
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
        Debug.Log(Settings.DroneBModel.name);
    }
}
