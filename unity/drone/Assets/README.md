# Unity Assets

## Asset Store

### EasyRoads3D v3.2.1f1
[EasyRoads3D](https://assetstore.unity.com/packages/3d/characters/easyroads3d-free-v3-987) was used to quickly and easily create road textures with compatibility with the Unity Terrain along with HDRP support, building a more realistic environment.

The Manual is located in the EasyRoads3D folder at [Quick Start.txt](EasyRoads3D/Quick%20Start.txt).

### Flexible Color Picker v2.5.0
[Flexible Color Picker](https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497) is a UI addon enabling the user to freely change the colour of a selected material. It is used in this project to modify color of the drone during runtime.

A guide is present in the FlexibleColorPicker folder at [FlexibleColorPickerDoc.pdf](FlexibleColorPicker/FlexibleColorPickerDoc.pdf)

## Runtime Importers

During runtime, 3D Models can be imported by the user through the StreamingAssets folder into the application and instantiated into the scene. This is achieved using the following assets:

### GLTFUtility v0.7 and JsonDotNet

[GLTFUtility](https://github.com/Siccity/GLTFUtility) (together with [JsonDotNet](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)) is used to import .glTF files from the StreamingAssets folder into the application. Read more on the [README in the folder](GLTFUtility/README.md) or on the GitHub repository itself.

A modification was made in the [Importer.cs](GLTFUtility/Scripts/Importer.cs) file to skip the importing of the materials. Instead, after the object is imported and instantiated, a custom material is applied to the object. This material's colour can also be changed with the [Flexible Color Picker](#flexible-color-picker-v250) as discussed above.

### Runtime OBJ Importer v2.02

[Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547) is used to import .obj files from the StreamingAssets folder into the application. Read more on the [README in the folder](OBJImport/README.HTML).

A custom material is also applied to the object after import, which similarly can be changed with the [Flexible Color Picker](#flexible-color-picker-v250).