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
    public static string TestCasesPath = Application.streamingAssetsPath + "/testdata/";
    public DroneController Target;
    public Camera Camera;
    public static string SaveFile;
    private static bool _saveStarted = false;
    private static StreamWriter _sw;

    public static string LoadFile;
    private static bool _loadStarted = false;
    private static StreamReader _sr;
    private static bool _passCase = true;

    public void Start()
    {
        RefreshTestCases();
    }

    void OnDisable()
    {
        StopLoadCase();
    }

    public void FixedUpdate()
    {
        if (_saveStarted)
        {
            if (_sw == null)
            {
                if (!Directory.Exists(TestCasesPath))
                {
                    Directory.CreateDirectory(TestCasesPath);
                }
                File.Delete(TestCasesPath + SaveFile + ".txt");
                _sw = File.AppendText(TestCasesPath + SaveFile + ".txt");
            }
            // _sw.WriteLine(Target.Drone.transform.localPosition.ToString("f3"));
            Vector3 v = Target.Drone.GetComponent<Rigidbody>().velocity /Target.MaxSpeed;
            _sw.WriteLine(v);
        }

        if (_loadStarted)
        {
            string s;
            if (_sr == null)
            {
                _sr = File.OpenText(TestCasesPath + LoadFile + ".txt");
                s = "";
            }
            if ((s = _sr.ReadLine()) != null)
            {
                // Target.Drone.transform.localPosition = StringToVector3(s);
                Target.Drone.GetComponent<Rigidbody>().velocity = StringToVector3(s) * Target.MaxSpeed;
            }
            else
            {
                StopLoadCase();
            }
            if (!DroneCheck.CheckInFrame(Target.Drone, Camera))
            {
                // Debug.Log("fail: " + LoadFile);
                _passCase = false;
                StopLoadCase();
            }
        }
    }

    public static void RefreshTestCases()
    {
        TestCases = new List<string>(Directory.GetFiles(TestCasesPath, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)));
    }

    public static void SaveCase(string file)
    {
        if (!_loadStarted)
        {
            if (file == "")
            {
                int i = 1;
                file = String.Format("case{0}", i);
                while (File.Exists(TestCasesPath + file + ".txt")) file = String.Format("case{0}", ++i); ;
            }
            SaveFile = file;
            _saveStarted = true;
        }
    }

    public static void StopSaveCase()
    {
        _saveStarted = false;
        if (_sw != null) _sw.Dispose();
        _sw = null;
    }

    public static async Task<bool> LoadCase(string file)
    {
        ResetObjects.Restart();
        LoadFile = file;
        _loadStarted = true;
        _passCase = true;
        while (_loadStarted)
        {
            await Task.Delay(100);
        }
        return _passCase;
        // await Task.CompletedTask;
    }

    public static void StopLoadCase()
    {
        _loadStarted = false;
        if (_sr != null) _sr.Dispose();
        _sr = null;
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