using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCP_Material : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public Material material;
    void Update()
    {
        material.color = fcp.color;
    }
}
