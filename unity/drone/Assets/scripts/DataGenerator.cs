using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class DataGenerator : MonoBehaviour
{
    public GameObject TargetDrone;
    public Camera cam;
    public GameObject Light;
    public GameObject Terrain;
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
        if (frame == 60)
        {
            frame = 0;
            if (Light)
            {
                Light.GetComponent<Randomizer>().Randomize();
            }
            if (Terrain)
            {
                Terrain.GetComponent<Randomizer>().Randomize();
            }
            TargetDrone.GetComponent<Randomizer>().Randomize();

            if (CheckInFrame(TargetDrone, cam))
            {
                GenerateOutput(cam);
            }
            // CheckInFrame(TargetDrone, cam);
        }

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

        // if bounding box completely within screen edges
        if (0 <= minX && minX <= screenWidth && 0 <= minY && minY <= screenHeight && 0 <= maxX && maxX <= screenWidth && 0 <= maxY && maxY <= screenHeight)
        {
            return true;
        }
        return false;

    }

    public void GenerateOutput(Camera Camera)
    {
        // outputs camera image to jpg
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;

        Camera.Render();

        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToJPG(); // change to .EncodeToPNG() for png files
        Destroy(image);

        File.WriteAllBytes(Application.dataPath + "/../frames/test" + fileCounter + ".jpg", bytes); // change to .png for png files

        // outputs position data in specific format
        File.WriteAllText(Application.dataPath + "/../frames/test" + fileCounter + ".txt", "0.0 0 0.0 " + minX + " " + minY + " " + maxX + " " + maxY + " 0.0 0.0 0.0 0.0 0.0 0.0 0.0");

        fileCounter++;
    }
}
