using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    WaypointManager waypointManager = new WaypointManager();
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
        waypointManager.RefreshWaypoints();
        foreach (Transform child in WaypointScrollContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (string waypoint in waypointManager.Waypoints)
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

    public async void LoadWaypoint(string waypointFilename)
    {
        waypointManager.StopSaveWaypoint();
        waypointManager.StopLoadWaypoint();
        while (waypointManager.WaypointLoadStarted)
        {
            await Task.Delay(100);
        }
        OutputText.text = "Attempting to load " + waypointFilename;
        await waypointManager.LoadWaypoint(waypointFilename, Target);
        OutputText.text = "Finished loading " + waypointFilename;
    }
    public void SaveWaypoint(bool isOn)
    {
        if (isOn)
        {
            string file = FileInput.text;
            if (file == "")
            {
                file = WaypointManager.GenerateFilename(waypointManager.WaypointsPath, "waypoint");
            }
            waypointManager.StopLoadWaypoint();
            waypointManager.SaveWaypoint(file, Target);
            OutputText.text = "Attempting to save to " + file;
        }

    }
    public void StopSaveWaypoint(bool isOn)
    {
        if (isOn)
        {
            waypointManager.StopSaveWaypoint();
            OutputText.text = "Attempting to stop saving";
            updateWaypoints();
        }

    }
}
