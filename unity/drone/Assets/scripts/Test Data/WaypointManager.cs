using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UnityEngine;

public class WaypointManager
{
    public List<string> Waypoints;
    public string WaypointsPath
    {
        get { return Application.streamingAssetsPath + "/waypoint/"; }
    }
    // save waypoint
    private bool s_waypointSaveStarted = false;
    private StreamWriter s_waypointSw;
    // load waypoint
    public bool WaypointLoadStarted = false;
    private StreamReader s_waypointSr;

    public void RefreshWaypoints()
    {
        if (Directory.Exists(WaypointsPath))
        {
            // retrieves all txt files in the Waypoint directory
            Waypoints = new List<string>(Directory.GetFiles(WaypointsPath, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)));
        }
        else
        {
            Directory.CreateDirectory(WaypointsPath);
        }
    }

    public async void SaveWaypoint(string file, DroneController target)
    {
        if (!s_waypointSaveStarted && !WaypointLoadStarted)
        {
            if (file == "")
            {
                file = GenerateFilename(WaypointsPath, "waypoint");
            }
            await saveWaypoint(file, target);
        }
    }
    public static string GenerateFilename(string path, string prefix)
    {
        int i = 1;
        string file = $"{prefix}{i}";
        while (File.Exists(path + file + ".txt")) file = $"{prefix}{++i}";
        return file;
    }
    async Task saveWaypoint(string file, DroneController target)
    {
        Vector3 initialPosition;
        Vector3 previousVelocity;
        if (!Directory.Exists(WaypointsPath))
        {
            Directory.CreateDirectory(WaypointsPath);
        }
        // delete and overwrite save file if it already exists
        File.Delete(WaypointsPath + file + ".txt");
        // initialise stream writer
        s_waypointSw = File.AppendText(WaypointsPath + file + ".txt");
        initialPosition = target.Drone.transform.localPosition;
        previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
        Dictionary<KeyCode, bool> keyboard = new Dictionary<KeyCode, bool>
        {
            { KeyCode.W, false},
            { KeyCode.A, false},
            { KeyCode.S, false},
            { KeyCode.D, false},
        };
        while (s_waypointSw != null)
        {
            bool changedKey = false;
            // if (target.Drone.GetComponent<Rigidbody>().velocity != previousVelocity)
            // if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.D))
            List<KeyCode> keys = new List<KeyCode>(keyboard.Keys);
            foreach (KeyCode key in keys)
            {
                if (Input.GetKey(key) != keyboard[key])
                {
                    keyboard[key] = Input.GetKey(key);
                    changedKey = true;
                }
            }
            if (changedKey && previousVelocity.magnitude > 0)
            {
                // Debug.Log("change detected");
                s_waypointSw.WriteLine($"{target.Drone.transform.localPosition.x - initialPosition.x},{target.Drone.transform.localPosition.z - initialPosition.z},{previousVelocity.magnitude},{initialPosition.y}");
                // previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
            }
            previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
            await Task.Delay(50);
        }
        StopSaveWaypoint();
    }

    public void StopSaveWaypoint()
    {
        s_waypointSaveStarted = false;
        if (s_waypointSw != null) s_waypointSw.Dispose();
        s_waypointSw = null;
    }
    public async Task LoadWaypoint(string file, DroneController target)
    {
        if (!s_waypointSaveStarted && !WaypointLoadStarted)
        {
            WaypointLoadStarted = true;
            ResetObjects.Restart();
            Debug.Log("LoadWaypoint");
            await loadWaypoint(file, target);
        }
    }
    async Task loadWaypoint(string file, DroneController target)
    {
        Debug.Log("load waypoint");
        string s;
        Rigidbody rb = target.Drone.GetComponent<Rigidbody>();
        float[] prev = new float[] { 0, 0 };
        // initialise stream reader
        s_waypointSr = File.OpenText(WaypointsPath + file + ".txt");
        s = "";
        target.GetComponent<KeyboardController>().enabled = false;

        while (s_waypointSr != null && (s = s_waypointSr.ReadLine()) != null)
        {
            Debug.Log(s);

            // format : {x, y, v}
            string[] inputs = s.Split(',');
            float dx = float.Parse(inputs[0]) - prev[0];
            float dy = float.Parse(inputs[1]) - prev[1];
            float dv = float.Parse(inputs[2]);

            prev = new float[] { float.Parse(inputs[0]), float.Parse(inputs[1]) };

            float totalDistance = Mathf.Sqrt(dx * dx + dy * dy);

            float vx = dv * dx / totalDistance;
            float vy = dv * dy / totalDistance;
            float waitTime = totalDistance / dv;

            // rb.drag = 0;
            // rb.velocity = new Vector3(vx, 0, vy);
            vx /= target.MaxSpeed;
            vy /= target.MaxSpeed;
            

            float timeElapsed = 0;
            Debug.Log("waiting for " + waitTime);
            // yield return new WaitForSecondsRealtime(waitTime);
            while (s_waypointSr != null && timeElapsed < waitTime)
            {
                // VelocityConverter.Convert(rb, vx, vy, target.MaxSpeed, 25, 1);
                target.GetComponent<VelocityConverter>().SetVelocities(vx, vy);
                timeElapsed += Time.fixedDeltaTime;
                await Task.Delay(Mathf.RoundToInt(Time.fixedDeltaTime * 1000));
            }
            // await Task.Delay(Mathf.RoundToInt(waitTime * 1000));
            Debug.Log("waiting finished");
        }

        // once stream reader reaches an empty line
        Debug.Log("stop load case");
        VelocityConverter.Convert(rb, 0, 0, target.MaxSpeed, 25, 1);
        // rb.velocity = new Vector3(0, 0, 0);
        target.GetComponent<KeyboardController>().enabled = true;
        StopLoadWaypoint();
        WaypointLoadStarted = false;
    }
    public void StopLoadWaypoint()
    {
        // WaypointLoadStarted = false;
        if (s_waypointSr != null) s_waypointSr.Dispose();
        s_waypointSr = null;
    }
}
