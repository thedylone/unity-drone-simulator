using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Dummiesman;
using Siccity.GLTFUtility;

public class WindUI : MonoBehaviour
{
    public Dropdown windLevelDropDown;

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            PlayerPrefs.SetInt("wind level", 0);
        }
        else if (val == 1)
        {
            PlayerPrefs.SetInt("wind level", 1);
        }
        else if (val == 2)
        {
            PlayerPrefs.SetInt("wind level", 2);
        }
        else if (val == 3)
        {
            PlayerPrefs.SetInt("wind level", 3);
        }
    }
}
