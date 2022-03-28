using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public bool RandomPosition;
    public float MinPosX;
    public float MaxPosX;
    public float MinPosY;
    public float MaxPosY;
    public float MinPosZ;
    public float MaxPosZ;
    public bool RandomRotation;
    public float MinRotX;
    public float MaxRotX;
    public float MinRotY;
    public float MaxRotY;
    public float MinRotZ;
    public float MaxRotZ;

    public void Randomize()
    {
        if (RandomPosition)
        {
            var pos = new Vector3(Random.Range(MinPosX, MaxPosX), Random.Range(MinPosY, MaxPosY), Random.Range(MinPosZ, MaxPosZ));
            transform.position = pos;
        }
        if (RandomRotation)
        {
            var rot = new Vector3(Random.Range(MinRotX, MaxRotX), Random.Range(MinRotY, MaxRotY), Random.Range(MinRotZ, MaxRotZ));
            transform.Rotate(rot);
        }
    }
}
