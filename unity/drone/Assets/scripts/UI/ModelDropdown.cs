using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelDropdown : MonoBehaviour
{
    public Dropdown Dropdown;
    public DroneController DroneController;
    private GameObject[] DroneArray;
    void Start()
    {
        DroneArray = GameObject.FindGameObjectsWithTag("Model");
        addDropdown();
        Dropdown.onValueChanged.AddListener(delegate {
            updateDropdown();
        });
        for (int i = 1; i < DroneArray.Length; i++)
        {
            DroneArray[i].SetActive(false);
        }
        DroneController.Drone = DroneArray[0];
    }
    void addDropdown()
    {
        List<string> options = new List<string>();
        foreach(var option in DroneArray)
        {
            options.Add(option.name);
        }
        Dropdown.ClearOptions();
        Dropdown.AddOptions(options);
    }
    void updateDropdown()
    {
        // Debug.Log("updated");
        foreach(var option in DroneArray)
        {
            option.SetActive(false);
        }
        // Debug.Log(Dropdown.captionText.text);
        GameObject selectedDrone = DroneArray[Dropdown.value];
        DroneController.Drone = selectedDrone;
        selectedDrone.SetActive(true);
    }
}
