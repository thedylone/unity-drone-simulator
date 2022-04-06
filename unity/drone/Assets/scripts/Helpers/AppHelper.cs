using UnityEngine;
using System.Collections;

public class AppHelper : MonoBehaviour
{
#if UNITY_WEBPLAYER
    public static string webplayerQuitURL = "http://google.com";
#endif
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }
}
