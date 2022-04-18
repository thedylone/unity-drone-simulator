using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DroneCheck
{
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

        // use this if bounding box is completely within screen edges
        // target has to be fully visible, i.e. if part of the target
        // is cut off by edge of screen, will not be detected
        // if (0 <= minX && 0 <= minY && maxX <= screenWidth && maxY <= screenHeight)
        // {
        //     RaycastHit hit;
        //     if (Physics.Linecast(camera.transform.position, c, out hit))
        //     { if (hit.transform.name != target.name) return false; }
        //     return true;
        // }
        // return false;

        // use this instead if centre of target is within the screen edges
        // Vector3 screenPoint = camera.WorldToViewportPoint(c);
        // bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        // if (onScreen)
        // {
        //     RaycastHit hit;
        //     if (Physics.Linecast(camera.transform.position, c, out hit))
        //     { if (hit.transform.name != target.name) return false; }
        //     return true;
        // }
        // return false;

        // use this instead if target should be detected even if
        // only a small part of the target is on screen
        if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), target.GetComponent<Renderer>().bounds))
        {
            RaycastHit hit;
            if (Physics.Linecast(camera.transform.position, c, out hit))
            { if (hit.transform.name != target.name) return false; }
            return true;
        }
        return false;
    }
}
