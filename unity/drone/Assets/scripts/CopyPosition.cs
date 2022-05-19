using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public GameObject Parent;
    void Update()
    {
        transform.position = Parent.transform.position;   
    }
}
