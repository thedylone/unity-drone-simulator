using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class TestCaseManager : MonoBehaviour
{
    public static List<string> TestCases;
    public static List<string> Waypoints;
    public static string TestCasesPath = Application.streamingAssetsPath + "/testdata/";
    public static string WaypointsPath = Application.streamingAssetsPath + "/waypoint/";
    public DroneController Target;
    public Camera Camera;
    // save test data
    public static string SaveFile;
    private static bool s_saveStarted = false;
    private static StreamWriter s_sw;
    // load test data
    public static string LoadFile;
    private static bool s_loadStarted = false;
    private static StreamReader s_sr;
    private static bool s_passCase = true;
    // load waypoint
    public static string WaypointFile;
    private static bool s_waypointStarted = false;
    private static StreamReader s_waypointSr;

    public void Start()
    {
        RefreshTestCases();
        RefreshWaypoints();
    }

    void OnDisable()
    {
        StopLoadCase();
        StopLoadWaypoint();
    }

    public void FixedUpdate()
    {
        if (s_saveStarted)
        {
            if (s_sw == null)
            {
                // if save hasn't started yet
                if (!Directory.Exists(TestCasesPath))
                {
                    Directory.CreateDirectory(TestCasesPath);
                }
                // delete and overwrite save file if it already exists
                File.Delete(TestCasesPath + SaveFile + ".txt");
                // initialise stream writer
                s_sw = File.AppendText(TestCasesPath + SaveFile + ".txt");
            }
            // save the drone's desired property to the file
            // s_sw.WriteLine(Target.Drone.transform.localPosition.ToString("f3"));
            Vector3 v = Target.Drone.GetComponent<Rigidbody>().velocity / Target.MaxSpeed;
            s_sw.WriteLine(v);
        }

        if (s_loadStarted)
        {
            string s;
            if (s_sr == null)
            {
                // if load hasn't started yet
                // initialise stream reader
                s_sr = File.OpenText(TestCasesPath + LoadFile + ".txt");
                s = "";
            }
            if ((s = s_sr.ReadLine()) != null)
            {
                // Target.Drone.transform.localPosition = StringToVector3(s);
                Target.Drone.GetComponent<Rigidbody>().velocity = StringToVector3(s) * Target.MaxSpeed;
            }
            else
            {
                // once stream reader reaches an empty line
                StopLoadCase();
            }
            // check if the drone is in frame, if not then stop running and treat as fail
            if (!DroneCheck.CheckInFrame(Target.Drone, Camera))
            {
                // Debug.Log("fail: " + LoadFile);
                s_passCase = false;
                StopLoadCase();
            }
        }
        if (s_waypointStarted && s_waypointSr == null) StartCoroutine(LoadWaypoint());
    }

    IEnumerator LoadWaypoint()
    {
        string s;
        Rigidbody rb = Target.Drone.GetComponent<Rigidbody>();
        float[] prev = new float[] { 0, 0 };
        if (s_waypointSr == null)
        {
            // if load hasn't started yet
            // initialise stream reader
            s_waypointSr = File.OpenText(WaypointsPath + WaypointFile + ".txt");
            s = "";
            Target.GetComponent<KeyboardController>().enabled = false;
        }
        while ((s = s_waypointSr.ReadLine()) != null)
        {
            Debug.Log(s);
            // check if the drone is in frame, if not then stop running and treat as fail
            if (!DroneCheck.CheckInFrame(Target.Drone, Camera))
            {
                // Debug.Log("fail: " + LoadFile);
                s_passCase = false;
                break;
            }
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

            rb.drag = 0;
            rb.velocity = new Vector3(vx, 0, vy);

            Debug.Log("waiting for " + waitTime);
            yield return new WaitForSecondsRealtime(waitTime);
            Debug.Log("waiting finished");
        }

        // once stream reader reaches an empty line
        Debug.Log("stop load case");
        rb.velocity = new Vector3(0, 0, 0);
        Target.GetComponent<KeyboardController>().enabled = true;
        StopLoadWaypoint();
    }

    public static void RefreshTestCases()
    {
        if (Directory.Exists(TestCasesPath))
        {
            // retrieves all txt files in the Test Case directory
            TestCases = new List<string>(Directory.GetFiles(TestCasesPath, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)));
        }
        else
        {
            Directory.CreateDirectory(TestCasesPath);
        }
    }

    public static void RefreshWaypoints()
    {
        if (Directory.Exists(TestCasesPath))
        {
            // retrieves all txt files in the Waypoint directory
            Waypoints = new List<string>(Directory.GetFiles(WaypointsPath, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)));
        }
        else
        {
            Directory.CreateDirectory(TestCasesPath);
        }
    }
    public static void SaveCase(string file)
    {
        if (!s_loadStarted)
        {
            if (file == "")
            {
                // automatically assign a name for the file if no file name is provided
                // file name will be formatted as case1.txt for example
                int i = 1;
                file = String.Format("case{0}", i);
                while (File.Exists(TestCasesPath + file + ".txt")) file = String.Format("case{0}", ++i); ;
            }
            SaveFile = file;
            s_saveStarted = true;
        }
    }

    public static void StopSaveCase()
    {
        s_saveStarted = false;
        if (s_sw != null) s_sw.Dispose();
        s_sw = null;
    }

    public static async Task<bool> LoadCase(string file)
    {
        // returns true if case passed, else false if case failed
        ResetObjects.Restart();
        LoadFile = file;
        s_loadStarted = true;
        s_passCase = true;
        while (s_loadStarted)
        {
            // waits for load to complete/stop
            await Task.Delay(100);
        }
        return s_passCase;
        // await Task.CompletedTask;
    }

    public static void StopLoadCase()
    {
        s_loadStarted = false;
        if (s_sr != null) s_sr.Dispose();
        s_sr = null;
    }

    public static async Task<bool> LoadWaypoint(string file)
    {
        // returns true if case passed, else false if case failed
        ResetObjects.Restart();
        WaypointFile = file;
        s_waypointStarted = true;
        s_passCase = true;
        while (s_waypointStarted)
        {
            // waits for load to complete/stop
            await Task.Delay(100);
        }
        return s_passCase;
        // await Task.CompletedTask;
    }
    public static void StopLoadWaypoint()
    {
        s_waypointStarted = false;
        if (s_waypointSr != null) s_waypointSr.Dispose();
        s_waypointSr = null;
    }
    public static Vector3 StringToVector3(string sVector)
    {
        // remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static bool CheckInFrame(GameObject target, Camera camera)
    {
        camera = camera.GetComponent<Camera>();
        float screenWidth = camera.pixelWidth;
        float screenHeight = camera.pixelHeight;

        Vector3 c = target.GetComponent<Renderer>().bounds.center;
        Vector3 e = target.GetComponent<Renderer>().bounds.extents;

        // positions of corners of bounding box
        Vector3[] worldCorners = new[] {
            new Vector3( c.x + e.x, c.y + e.y, c.z + e.z ),
            new Vector3( c.x + e.x, c.y + e.y, c.z - e.z ),
            new Vector3( c.x + e.x, c.y - e.y, c.z + e.z ),
            new Vector3( c.x + e.x, c.y - e.y, c.z - e.z ),
            new Vector3( c.x - e.x, c.y + e.y, c.z + e.z ),
            new Vector3( c.x - e.x, c.y + e.y, c.z - e.z ),
            new Vector3( c.x - e.x, c.y - e.y, c.z + e.z ),
            new Vector3( c.x - e.x, c.y - e.y, c.z - e.z ),
        };
        // convert from world position to screen position
        IEnumerable<Vector3> screenCorners = worldCorners.Select(corner => camera.WorldToScreenPoint(corner));
        float maxX = screenCorners.Max(corner => corner.x);
        float minX = screenCorners.Min(corner => corner.x);
        float maxY = screenCorners.Max(corner => corner.y);
        float minY = screenCorners.Min(corner => corner.y);

        /* if bounding box completely within screen edges
        target has to be fully visible, i.e. if part of the target
        is cut off by edge of screen, will not be detected */
        if (0 <= minX && 0 <= minY && maxX <= screenWidth && maxY <= screenHeight)
        {
            RaycastHit hit;
            // Debug.DrawLine(camera.transform.position, c, Color.red);
            if (Physics.Linecast(camera.transform.position, c, out hit))
            { if (hit.transform.name != target.name) return false; }
            return true;
        }
        return false;

        /* use this instead if target should be detected even if
        only a small part of the target is on screen

        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), target.GetComponent<Renderer>().bounds))
        {
            RaycastHit hit;
            Debug.DrawLine(camera.transform.position, c, Color.red);
            if (Physics.Linecast(camera.transform.position, c, out hit))
            { if (hit.transform.name != target.name) return false; }
            return true;
        }
        return false;

        OR

        Vector3 screenPoint = camera.WorldToViewportPoint(c);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen)
        {
            RaycastHit hit;
            Debug.DrawLine(camera.transform.position, c, Color.red);
            if (Physics.Linecast(camera.transform.position, c, out hit))
            { if (hit.transform.name != target.name) return false; }
            return true;
        }
        return false;
        */

    }
}
