using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCaseUI : MonoBehaviour
{
    public Text OutputText;
    public InputField FileInput;
    public Button SaveButton;
    public Button LoadButton;
    public Button LoadAllButton;
    public GameObject ScrollContent;
    public GameObject PrefabPanel;
    private bool _saveStarted = false;
    private bool _loadStarted = false;
    void Start()
    {
        TestCaseManager.RefreshTestCases();
        foreach (string testcase in TestCaseManager.TestCases)
        {
            var panel = Object.Instantiate(PrefabPanel, Vector3.zero, Quaternion.identity) as GameObject;
            panel.transform.SetParent(ScrollContent.transform, false);
            panel.GetComponentInChildren<Text>().text = testcase;
            var buttons = panel.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(delegate {
                SetFileInput(testcase);
            });
            buttons[1].onClick.AddListener(delegate {
                ToggleLoad(testcase);
            });
        }
    }

    public void SetFileInput(string _filename)
    {
        FileInput.text = _filename;
    }

    public void ToggleSave()
    {
        if (!_loadStarted)
        {
            _saveStarted = !_saveStarted;
            if (_saveStarted)
            {
                SaveButton.GetComponentInChildren<Text>().text = "stop save";
                TestCaseManager.SaveCase(FileInput.text);
            }
            else
            {
                SaveButton.GetComponentInChildren<Text>().text = "start save";
                TestCaseManager.StopSaveCase();
            }
        }
    }

    public async void ToggleLoad(string _filename)
    {
        if (!_saveStarted)
        {
            _loadStarted = !_loadStarted;
            if (_loadStarted)
            {
                OutputText.text = "running " + _filename;
                OutputText.text = _filename + (await TestCaseManager.LoadCase(_filename) ? " passed" : " failed");
                _loadStarted = false;
            }
            else
            {
                TestCaseManager.StopLoadCase();
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
                ColorBlock _cb = LoadAllButton.colors;
                _cb.normalColor = Color.gray;
                _cb.selectedColor = Color.gray;
                LoadAllButton.colors = _cb;
                TestCaseManager.RefreshTestCases();
                OutputText.text = "running all cases\n";
                foreach (string file in TestCaseManager.TestCases)
                {
                    if (_loadStarted) OutputText.text += file + (await TestCaseManager.LoadCase(file) ? " passed\n" : " failed\n");
                }
                OutputText.text += "running completed";
                _loadStarted = false;
            }
            else
            {
                LoadAllButton.GetComponentInChildren<Text>().text = "start load";
                LoadAllButton.colors = ColorBlock.defaultColorBlock;
                TestCaseManager.StopLoadCase();
            }
        }
    }
}
