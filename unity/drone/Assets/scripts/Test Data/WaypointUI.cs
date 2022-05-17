using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointUI : MonoBehaviour
{
    public DroneController Target;
    public Text OutputText;
    public GameObject WaypointScrollContent;
    public InputField FileInput;
    public Toggle SaveButton;
    public Toggle StopSaveButton;
    public GameObject PrefabPanel;
    // private bool _saveStarted = false;
    // private bool _loadStarted = false;

    void Start()
    {
        SaveButton.onValueChanged.AddListener(SaveWaypoint);
        StopSaveButton.onValueChanged.AddListener(StopSaveWaypoint);
        updateWaypoints();
    }
    void updateWaypoints()
    {
        WaypointManager.RefreshWaypoints();
        foreach (Transform child in WaypointScrollContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (string waypoint in WaypointManager.Waypoints)
        {
            var panel = Object.Instantiate(PrefabPanel, Vector3.zero, Quaternion.identity) as GameObject;
            panel.transform.SetParent(WaypointScrollContent.transform, false);
            panel.GetComponentInChildren<Text>().text = waypoint;
            var buttons = panel.GetComponentsInChildren<Button>();
            buttons[1].onClick.AddListener(delegate
            {
                LoadWaypoint(waypoint);
            });
        }
    }

    public void LoadWaypoint(string waypointFilename)
    {
        WaypointManager.StopSaveWaypoint();
        WaypointManager.StopLoadWaypoint();
        OutputText.text = "Attempting to load " + waypointFilename;
        WaypointManager.LoadWaypoint(waypointFilename, Target);
    }
    public void SaveWaypoint(bool isOn)
    {
        if (isOn)
        {
            string file = FileInput.text;
            if (file == "")
            {
                file = WaypointManager.GenerateFilename(WaypointManager.WaypointsPath, "waypoint");
            }
            WaypointManager.StopLoadWaypoint();
            WaypointManager.SaveWaypoint(file, Target);
            OutputText.text = "Attempting to save to " + file;
        }

    }
    public void StopSaveWaypoint(bool isOn)
    {
        if (isOn)
        {
            WaypointManager.StopSaveWaypoint();
            OutputText.text = "Attempting to stop saving";
            updateWaypoints();
        }

    }
}
