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
    static string droneModelsPath = Application.streamingAssetsPath + "/Drone Models/";
    Shader hdrpLit;
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
        DroneBDropdown.value = Mathf.Min(PlayerPrefs.GetInt(DroneBDropdown.name), DroneBDropdown.options.Count);
        dropdownChange(DroneBDropdown.value);
    }
    public void RefreshDropdown()
    {
        loadResources();
        updateDropdownOptions();
    }
    void loadResources()
    {
        // hdrpLit = Shader.Find("HDRP/Lit");

        droneModels = new List<GameObject>(Resources.LoadAll<GameObject>("Drone Models"));

        // import from streamingassets
        if (Directory.Exists(droneModelsPath))
        {
            foreach (string file in Directory.GetFiles(droneModelsPath).Where(name => !name.EndsWith(".meta")))
            {
                if (Path.GetFileName(file).ToUpper().EndsWith(".OBJ"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    if (GameObject.Find(name)) Destroy(GameObject.Find(name));
                    GameObject obj = new OBJLoader().Load(file);
                    obj.name = name;

                    foreach (MeshRenderer childRenderer in obj.GetComponentsInChildren<MeshRenderer>()) childRenderer.material = DroneMaterial;
                    if (obj.TryGetComponent(out MeshRenderer renderer)) renderer.material = DroneMaterial;
                    droneModels.Add(obj);
                    DontDestroyOnLoad(obj);

                }
                else if (Path.GetFileName(file).ToUpper().EndsWith(".GLB") || Path.GetFileName(file).ToUpper().EndsWith(".GLTF"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    if (GameObject.Find(name)) Destroy(GameObject.Find(name));
                    GameObject gltfObject = Importer.LoadFromFile(file);
                    gltfObject.name = name;

                    foreach (MeshRenderer childRenderer in gltfObject.GetComponentsInChildren<MeshRenderer>()) childRenderer.material = DroneMaterial;
                    if (gltfObject.TryGetComponent(out MeshRenderer renderer)) renderer.material = DroneMaterial;
                    droneModels.Add(gltfObject);
                    DontDestroyOnLoad(gltfObject);

                }
            }
        }
        else
        {
            Directory.CreateDirectory(droneModelsPath);
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
        OutputText.text = "Drone model selected:\n" + Settings.DroneBModel.name;
    }
}
