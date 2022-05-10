using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Dummiesman;
using Siccity.GLTFUtility;
public class DronesUI : MonoBehaviour
{
    public InputField DistanceField;
    public InputField DroneASpeedField;
    public InputField DroneBSpeedField;
    public Dropdown DroneBDropdown;
    public Text OutputText;
    List<GameObject> droneModels;
    public Camera PreviewCamera;
    GameObject previewModel;
    private static string s_droneModelsPath = Application.streamingAssetsPath + "/Drone Models/";
    public Material DroneMaterial;
    void Start()
    {
        distanceChange(DistanceField.text);
        droneAChange(DroneASpeedField.text);
        droneBChange(DroneBSpeedField.text);
        RefreshDropdown();
        DistanceField.onEndEdit.AddListener(distanceChange);
        DroneASpeedField.onEndEdit.AddListener(droneAChange);
        DroneBSpeedField.onEndEdit.AddListener(droneBChange);
        DroneBDropdown.onValueChanged.AddListener(dropdownChange);
        // Limit Drone B dropdown value to the max number of items (if number of drone models decreases)
        DroneBDropdown.value = Mathf.Min(PlayerPrefs.GetInt(DroneBDropdown.name), DroneBDropdown.options.Count);
        dropdownChange(DroneBDropdown.value);
    }
    public void RefreshDropdown()
    {
        loadResources();
        updateDropdownOptions();
        // reset Preview to show first item as dropdown resets to first item
        updatePreview(droneModels[0]);
    }
    void loadResources()
    {
        // loads drone models from internal "Drone Models" folder in Resources
        droneModels = new List<GameObject>(Resources.LoadAll<GameObject>("Drone Models"));

        // import from streamingassets
        if (Directory.Exists(s_droneModelsPath))
        {
            foreach (string file in Directory.GetFiles(s_droneModelsPath).Where(name => !name.EndsWith(".meta")))
            {
                if (Path.GetFileName(file).ToUpper().EndsWith(".OBJ"))
                {
                    // import OBJ file
                    string name = Path.GetFileNameWithoutExtension(file);
                    // destroy existing if already imported before
                    // to prevent multiple objects from stacking up
                    if (GameObject.Find(name)) Destroy(GameObject.Find(name));
                    GameObject obj = new OBJLoader().Load(file);
                    obj.name = name;

                    // changes the object's material to Drone Material to:
                    // 1. be able to display with HDRP
                    // 2. change the colour of the model
                    foreach (MeshRenderer childRenderer in obj.GetComponentsInChildren<MeshRenderer>()) childRenderer.material = DroneMaterial;
                    droneModels.Add(obj);
                    // Don't Destroy to allow Instantiating during Scene Switching
                    DontDestroyOnLoad(obj);

                }
                else if (Path.GetFileName(file).ToUpper().EndsWith(".GLB") || Path.GetFileName(file).ToUpper().EndsWith(".GLTF"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    // destroy existing if already imported before
                    // to prevent multiple objects from stacking up
                    if (GameObject.Find(name)) Destroy(GameObject.Find(name));
                    GameObject gltfObject = Importer.LoadFromFile(file);
                    gltfObject.name = name;

                    // changes the object's material to Drone Material to:
                    // 1. be able to display with HDRP
                    // 2. change the colour of the model
                    foreach (MeshRenderer childRenderer in gltfObject.GetComponentsInChildren<MeshRenderer>()) childRenderer.material = DroneMaterial;
                    droneModels.Add(gltfObject);
                    // Don't Destroy to allow Instantiating during Scene Switching
                    DontDestroyOnLoad(gltfObject);

                }
            }
        }
        else
        {
            Directory.CreateDirectory(s_droneModelsPath);
        }
    }
    void distanceChange(string distance)
    {
        Settings.DroneDistance = float.Parse(distance);
    }
    void droneAChange(string speed)
    {
        Settings.DroneASpeed = float.Parse(speed);
    }
    void droneBChange(string speed)
    {
        Settings.DroneBSpeed = float.Parse(speed);
    }
    void updateDropdownOptions()
    {
        DroneBDropdown.GetComponent<Dropdown>().ClearOptions();
        DroneBDropdown.GetComponent<Dropdown>().AddOptions(new List<string>(droneModels.Select(x => x.name)));
    }
    void dropdownChange(int index)
    {
        Settings.DroneBModel = droneModels[index];
        updatePreview(droneModels[index]);
        OutputText.text = "Drone model selected:\n" + Settings.DroneBModel.name;
    }
    void updatePreview(GameObject model)
    {
        // destroy existing model
        if (previewModel) Destroy(previewModel);
        // instantiate the model at the target location in view of the preview camera
        previewModel = Instantiate(model, new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 0));
        // prevent collision of the model
        if (previewModel.TryGetComponent<Rigidbody>(out Rigidbody rb)) Destroy(rb);
        // get the largest sizes of the model
        float sizeX = previewModel.GetComponentsInChildren<Renderer>().Select(c => c.bounds.size.x).Max();
        float sizeY = previewModel.GetComponentsInChildren<Renderer>().Select(c => c.bounds.size.y).Max();
        // float sizeZ = previewModel.GetComponentsInChildren<Renderer>().Select(c => c.bounds.size.z).Max();
        
        // move the camera to allow it to see the entire model
        float distance = Mathf.Max(sizeX, sizeY) / (2.0f * Mathf.Tan(0.5f * PreviewCamera.fieldOfView * Mathf.Deg2Rad));
        PreviewCamera.transform.position = new Vector3(PreviewCamera.transform.position.x, 100 + distance * 2.0f * Mathf.Tan(PreviewCamera.transform.eulerAngles.x * Mathf.PI / 180), 100 - distance * 2.0f);
        // PreviewCamera.transform.LookAt(previewModel.transform.position);
    }
}
