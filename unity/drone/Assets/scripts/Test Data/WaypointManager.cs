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
    public bool WaypointSaveStarted = false;
    private StreamWriter waypointSw;
    // load waypoint
    public bool WaypointLoadStarted = false;
    private StreamReader waypointSr;

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
        TestCaseManager testCaseManager;
        target.TryGetComponent<TestCaseManager>(out testCaseManager);
        if (!testCaseManager || !testCaseManager.SaveStarted)
        if (!WaypointSaveStarted && !WaypointLoadStarted)
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
        waypointSw = File.AppendText(WaypointsPath + file + ".txt");
        initialPosition = target.Drone.transform.localPosition;
        previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
        Dictionary<KeyCode, bool> keyboard = new Dictionary<KeyCode, bool>
        {
            { KeyCode.W, false},
            { KeyCode.A, false},
            { KeyCode.S, false},
            { KeyCode.D, false},
        };
        while (waypointSw != null)
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
                waypointSw.WriteLine($"{target.Drone.transform.localPosition.x - initialPosition.x},{target.Drone.transform.localPosition.z - initialPosition.z},{previousVelocity.magnitude},{initialPosition.y}");
                // previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
            }
            previousVelocity = target.Drone.GetComponent<Rigidbody>().velocity;
            await Task.Delay(50);
        }
        StopSaveWaypoint();
    }

    public void StopSaveWaypoint()
    {
        WaypointSaveStarted = false;
        if (waypointSw != null) waypointSw.Dispose();
        waypointSw = null;
    }
    public async Task LoadWaypoint(string file, DroneController target)
    {
        TestCaseManager testCaseManager;
        target.TryGetComponent<TestCaseManager>(out testCaseManager);
        if (!testCaseManager || !testCaseManager.LoadStarted)
        if (!WaypointSaveStarted && !WaypointLoadStarted)
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
        VelocityConverter vc = target.GetComponent<VelocityConverter>();
        float[] prev = new float[] { 0, 0 };
        Vector3 prevPosition = Vector3.zero;
        // initialise stream reader
        waypointSr = File.OpenText(WaypointsPath + file + ".txt");
        s = "";
        target.GetComponent<KeyboardController>().enabled = false;

        while (waypointSr != null && (s = waypointSr.ReadLine()) != null)
        {
            Debug.Log(s);

            // format : {x, y, v}
            string[] inputs = s.Split(',');
            float dx = float.Parse(inputs[0]) - prev[0];
            float dy = float.Parse(inputs[1]) - prev[1];
            float dv = float.Parse(inputs[2]);

            prev = new float[] { float.Parse(inputs[0]), float.Parse(inputs[1]) };
            prevPosition = rb.transform.position;

            float totalDistance = Mathf.Sqrt(dx * dx + dy * dy);

            float vx = dv * dx / totalDistance;
            float vy = dv * dy / totalDistance;
            float waitTime = totalDistance / dv;

            // rb.drag = 0;
            // rb.velocity = new Vector3(vx, 0, vy);
            vx /= target.MaxSpeed;
            vy /= target.MaxSpeed;
            

            // float timeElapsed = 0;
            // Debug.Log("waiting for " + waitTime);
            // // yield return new WaitForSecondsRealtime(waitTime);
            // while (waypointSr != null && timeElapsed < waitTime)
            // {
            //     // VelocityConverter.Convert(rb, vx, vy, target.MaxSpeed, 25, 1);
            //     vc.SetVelocities(vx, vy);
            //     timeElapsed += Time.fixedDeltaTime;
            //     await Task.Delay(Mathf.RoundToInt(Time.fixedDeltaTime * 1000));
            // }
            // // await Task.Delay(Mathf.RoundToInt(waitTime * 1000));
            // Debug.Log("waiting finished");
            float currentDistance = 0;
            while (waypointSr != null && currentDistance < totalDistance)
            {
                currentDistance = (rb.transform.position - prevPosition).magnitude;
                vc.SetVelocities(vx,vy);
                await Task.Delay(50);
            }
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
        if (waypointSr != null) waypointSr.Dispose();
        waypointSr = null;
    }
}
