using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCaseUI : MonoBehaviour
{
    public DroneController Target;
    public Camera Camera;
    public Text OutputText;
    public InputField FileInput;
    public Button SaveButton;
    public Button LoadButton;
    public Button LoadAllButton;
    public GameObject TestCaseScrollContent;
    public GameObject PrefabPanel;
    private bool _saveStarted = false;
    private bool _loadStarted = false;
    private string _currentFile;
    private TestCaseManager testCaseManager;
    void Start()
    {
        testCaseManager = Target.gameObject.AddComponent<TestCaseManager>();
        testCaseManager.Target = Target;
        testCaseManager.Camera = Camera;
        updateTestCases();
    }
    void OnDisable()
    {
        _saveStarted = false;
        _loadStarted = false;
    }
    void updateTestCases()
    {
        TestCaseManager.RefreshTestCases();
        foreach (string testcase in TestCaseManager.TestCases)
        {
            var panel = Object.Instantiate(PrefabPanel, Vector3.zero, Quaternion.identity) as GameObject;
            panel.transform.SetParent(TestCaseScrollContent.transform, false);
            panel.GetComponentInChildren<Text>().text = testcase;
            var buttons = panel.GetComponentsInChildren<Button>();
            // set first button to change the file input
            buttons[0].onClick.AddListener(delegate
            {
                SetFileInput(testcase);
            });
            // set second button to run the test case
            buttons[1].onClick.AddListener(delegate
            {
                ToggleLoad(testcase);
            });
        }
    }
    public void SetFileInput(string filename)
    {
        FileInput.text = filename;
    }
    public void ToggleSave()
    {
        if (!_loadStarted)
        {
            _saveStarted = !_saveStarted;
            if (_saveStarted)
            {
                // start save case
                SaveButton.GetComponentInChildren<Text>().text = "stop save";
                // check if OutputText exists due to switching scene
                if (OutputText) OutputText.text = "save in progress";
                testCaseManager.SaveCase(FileInput.text);
            }
            else
            {
                // stop save case
                SaveButton.GetComponentInChildren<Text>().text = "start save";
                // check if OutputText exists due to switching scene
                if (OutputText) OutputText.text = "save completed";
                testCaseManager.StopSaveCase();
                updateTestCases();
            }
        }
    }
    public async void ToggleLoad(string filename)
    {
        if (!_saveStarted)
        {
            if (_loadStarted && _currentFile != filename)
            {
                // if already loading a case and another case is loaded
                testCaseManager.StopLoadCase();
                _loadStarted = false;
            }
            _loadStarted = !_loadStarted;
            if (_loadStarted)
            {
                _currentFile = filename;
                OutputText.text = "running " + filename;
                string result = await testCaseManager.LoadCase(filename) ? " passed" : " failed";
                // check if OutputText exists due to switching scene
                if (OutputText) OutputText.text = filename + result;
                _loadStarted = false;
            }
            else
            {
                testCaseManager.StopLoadCase();
            }
        }
    }
    public async void ToggleLoadAll()
    {
        if (!_saveStarted)
        {
            _loadStarted = !_loadStarted;
            if (_loadStarted)
            {
                LoadAllButton.GetComponentInChildren<Text>().text = "stop load";
                // change the colour of the Load All button to show that it is in progress
                ColorBlock _cb = LoadAllButton.colors;
                _cb.normalColor = Color.gray;
                _cb.selectedColor = Color.gray;
                LoadAllButton.colors = _cb;
                TestCaseManager.RefreshTestCases();
                OutputText.text = "running all cases\n";
                foreach (string file in TestCaseManager.TestCases)
                {
                    if (_loadStarted)
                    {
                        // run each file if load is not cancelled
                        string result = (await testCaseManager.LoadCase(file) ? " passed" : " failed") + "\n";
                        // check if OutputText exists due to switching scene
                        if (OutputText) OutputText.text += file + result;
                    }
                }
                // check if OutputText exists due to switching scene
                if (OutputText) OutputText.text += "running completed";
                _loadStarted = false;
            }
            else
            {
                LoadAllButton.GetComponentInChildren<Text>().text = "start load";
                // reset Load All button colours
                LoadAllButton.colors = ColorBlock.defaultColorBlock;
                testCaseManager.StopLoadCase();
            }
        }
    }
}
