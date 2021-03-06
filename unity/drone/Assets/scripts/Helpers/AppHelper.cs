using UnityEngine;
using UnityEngine.SceneManagement;
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
    public static void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public static void SwitchScene(string sceneName)
    {
        Debug.Log("switching to "+sceneName);
        SceneManager.LoadScene(sceneName);   
    }
}
