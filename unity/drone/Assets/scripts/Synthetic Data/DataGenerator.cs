using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class DataGenerator : MonoBehaviour
{
    public GameObject TargetObject;
    public Camera TargetCamera;
    public Material Material;
    public int FilesLimit = 25;
    public string SavePath;
    public GameObject[] RandomizeObjects;
    public int MinRange = 10;
    public int MaxRange = 50;
    public int Step = 10;
    private float screenWidth;
    private float screenHeight;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private int fileCounter = 0;
    public int FileStartingNumber = 0;
    private int interval;
    private int frame = 0;

    void Update()
    {
        frame++;
        if (frame >= 2)
        {
            frame = 0;

            interval = Mathf.FloorToInt(fileCounter * (MaxRange - MinRange) / (FilesLimit * Step));

            Randomize();

            if (CheckInFrame(TargetObject, TargetCamera))
            {
                GenerateOutput(TargetCamera, SavePath);
                // Debug.Log("in frame");
            }
        }
        if (fileCounter >= FilesLimit) AppHelper.Quit(); // automatically stops running after set number of files

    }
    public bool CheckInFrame(GameObject target, Camera camera)
    {
        camera = camera.GetComponent<Camera>();
        screenWidth = camera.pixelWidth;
        screenHeight = camera.pixelHeight;
        List<Vector3> worldCorners = new List<Vector3>();
        List<Vector3> averageCentre = new List<Vector3>();
        foreach (Renderer renderer in target.GetComponentsInChildren<Renderer>())
        {
            Vector3 c = renderer.bounds.center;
            Vector3 e = renderer.bounds.extents;
            averageCentre.Add(c);
            worldCorners.AddRange(new[]
            {new Vector3(c.x + e.x, c.y + e.y, c.z + e.z),
            new Vector3(c.x + e.x, c.y + e.y, c.z - e.z),
            new Vector3(c.x + e.x, c.y - e.y, c.z + e.z),
            new Vector3(c.x + e.x, c.y - e.y, c.z - e.z),
            new Vector3(c.x - e.x, c.y + e.y, c.z + e.z),
            new Vector3(c.x - e.x, c.y + e.y, c.z - e.z),
            new Vector3(c.x - e.x, c.y - e.y, c.z + e.z),
            new Vector3(c.x - e.x, c.y - e.y, c.z - e.z)
            });
        }

        // Vector3 c = target.GetComponent<Renderer>().bounds.center;
        // Vector3 e = target.GetComponent<Renderer>().bounds.extents;

        // positions of corners of bounding box
        // Vector3[] worldCorners = new[] {
        //     new Vector3( c.x + e.x, c.y + e.y, c.z + e.z ),
        //     new Vector3( c.x + e.x, c.y + e.y, c.z - e.z ),
        //     new Vector3( c.x + e.x, c.y - e.y, c.z + e.z ),
        //     new Vector3( c.x + e.x, c.y - e.y, c.z - e.z ),
        //     new Vector3( c.x - e.x, c.y + e.y, c.z + e.z ),
        //     new Vector3( c.x - e.x, c.y + e.y, c.z - e.z ),
        //     new Vector3( c.x - e.x, c.y - e.y, c.z + e.z ),
        //     new Vector3( c.x - e.x, c.y - e.y, c.z - e.z ),
        // };
        // convert from world position to screen position
        IEnumerable<Vector3> screenCorners = worldCorners.Select(corner => camera.WorldToScreenPoint(corner));
        maxX = screenCorners.Max(corner => corner.x);
        minX = screenCorners.Min(corner => corner.x);
        maxY = screenCorners.Max(corner => corner.y);
        minY = screenCorners.Min(corner => corner.y);

        /* if bounding box completely within screen edges
        target has to be fully visible, i.e. if part of the target
        is cut off by edge of screen, will not be detected */
        if (0 <= minX && 0 <= minY && maxX <= screenWidth && maxY <= screenHeight)
        {
            RaycastHit hit;
            // Debug.DrawLine(camera.transform.position, c, Color.red);
            if (Physics.Linecast(camera.transform.position, new Vector3(averageCentre.Average(c => c.x), averageCentre.Average(c => c.y), averageCentre.Average(c => c.z)), out hit))
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
            if (Physics.Linecast(camera.transform.position, new Vector3(averageCentre.Average(c => c.x), averageCentre.Average(c => c.y), averageCentre.Average(c => c.z)), out hit))
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
            if (Physics.Linecast(camera.transform.position, new Vector3(averageCentre.Average(c => c.x), averageCentre.Average(c => c.y), averageCentre.Average(c => c.z)), out hit))
            { if (hit.transform.name != target.name) return false; }
            return true;
        }
        return false;
        */

    }

    public void GenerateOutput(Camera Camera, string SavePath)
    {
        if (SavePath.Length == 0) SavePath = Application.dataPath + "/generated/";
        SavePath += "/" + interval;
        Directory.CreateDirectory(SavePath);
        // outputs camera image to jpg
        if (Camera.targetTexture == null)
        {
            RenderTexture tempRT = new RenderTexture(Camera.pixelWidth, Camera.pixelHeight, 24, GetTargetFormat(Camera));
            tempRT.antiAliasing = GetAntiAliasingLevel(Camera);
            Camera.targetTexture = tempRT;
        }
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG(); // change to .EncodeToPNG() for png files
        Destroy(image);

        File.WriteAllBytes(SavePath + "/" + (fileCounter + FileStartingNumber) + ".png", bytes); // change to .png for png files

        // outputs position data in specific format
        File.WriteAllText(SavePath + "/" + (fileCounter + FileStartingNumber) + ".txt", "drone 0.0 0 0.0 " + Mathf.RoundToInt(minX).ToString("f2") + " " + Mathf.RoundToInt(screenHeight - maxY).ToString("f2") + " " + Mathf.RoundToInt(maxX).ToString("f2") + " " + Mathf.RoundToInt(screenHeight - minY).ToString("f2") + " 0.0 0.0 0.0 0.0 0.0 0.0 0.0");

        // ImageSynthesis synth = Camera.GetComponent<ImageSynthesis>();
        // synth.OnSceneChange();
        // synth.Save((fileCounter + FileStartingNumber).ToString(), 1920, 1080, path: SavePath);

        fileCounter++;
    }

    RenderTextureFormat GetTargetFormat(Camera camera)
    {
        return camera.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
    }

    int GetAntiAliasingLevel(Camera camera)
    {
        return camera.allowMSAA ? QualitySettings.antiAliasing : 1;
    }

    public void Randomize()
    {
        Material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        foreach (GameObject obj in RandomizeObjects)
        {
            Randomizer random = obj.GetComponent<Randomizer>();
            if (random.UseInterval)
            {
                random.MinPosY = MinRange + Step * interval;
                random.MaxPosY = MinRange + Step * (interval + 1);
            }
            random.Randomize();
        }
    }
}
