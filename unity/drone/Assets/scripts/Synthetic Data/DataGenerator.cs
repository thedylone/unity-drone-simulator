using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class DataGenerator : MonoBehaviour
{
    public GameObject TargetObject;
    public Camera TargetCamera;
    public int FilesLimit = 25;
    public string SavePath;
    public GameObject[] RandomizeObjects;
    private float screenWidth;
    private float screenHeight;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private int fileCounter = 0;
    private int frame = 0;

    void Update()
    {
        frame++;
        if (frame >= 2)
        {
            frame = 0;

            Randomize();

            if (CheckInFrame(TargetObject, TargetCamera))
            {
                GenerateOutput(TargetCamera);
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

    public void GenerateOutput(Camera Camera)
    {
        if (SavePath.Length == 0) SavePath = Application.dataPath;
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

        byte[] bytes = image.EncodeToJPG(); // change to .EncodeToPNG() for png files
        Destroy(image);

        File.WriteAllBytes(SavePath + "/" + fileCounter + ".jpg", bytes); // change to .png for png files

        // outputs position data in specific format
        File.WriteAllText(SavePath + "/" + fileCounter + ".txt", "drone 0.0 0 0.0 " + minX + " " + (screenHeight - maxY) + " " + maxX + " " + (screenHeight - minY) + " 0.0 0.0 0.0 0.0 0.0 0.0 0.0");

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
        foreach (GameObject obj in RandomizeObjects)
        {
            obj.GetComponent<Randomizer>().Randomize();
        }
    }
}
