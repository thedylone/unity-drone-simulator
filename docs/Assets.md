# Unity Assets

## Asset Store

### Unity Terrain - HDRP Demo Scene v1.0.0

[Unity Terrain - HDRP Demo Scene](https://assetstore.unity.com/packages/3d/environments/unity-terrain-hdrp-demo-scene-213198) was featured in the [Unity Blog showcasing the latest updates for HDRP](https://blog.unity.com/games/experience-the-new-unity-terrain-demo-scenes-for-hdrp-and-urp). Many assets were taken from this Demo Scene, including:

- Terrain Textures, Brushes
- Foliage, Trees, Grass
- HDRP Volume Prefabs
- ShaderGraphs

However, as the Demo is created for Unity v2021.2 and HDRP 12.1+, some functionality will not be available in this project's version of Unity v2020 and HDRP 10.8.1, such as:

- Volumetric Clouds
- Lens Flare

### EasyRoads3D v3.2.1f1

[EasyRoads3D](https://assetstore.unity.com/packages/3d/characters/easyroads3d-free-v3-987) was used to quickly and easily create road textures with compatibility with the Unity Terrain along with HDRP support, building a more realistic environment.

The Manual is located in the EasyRoads3D folder at [Quick Start.txt](../unity/drone/Assets/EasyRoads3D/Quick%20Start.txt).

### Flexible Color Picker v2.5.0

[Flexible Color Picker](https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497) is a UI addon enabling the user to freely change the colour of a selected material. It is used in this project to modify color of the drone during runtime.

A guide is present in the FlexibleColorPicker folder at [FlexibleColorPickerDoc.pdf](../unity/drone/Assets/FlexibleColorPicker/FlexibleColorPickerDoc.pdf)

### Street Lights Pack v1.0

[Street Lights Pack](https://assetstore.unity.com/packages/3d/props/exterior/street-lights-pack-31644) provided the model for the Street Lights located along the side of the road. For compatibility with HDRP, the shader of the material has to be changed to a HDRP Shader, e.g. HDRP/Lit.

### Yughues Free Sand Materials v1.0

[Yughues Free Sand Materials](https://assetstore.unity.com/packages/2d/textures-materials/floors/yughues-free-sand-materials-12964) was used to supplement the Terrain Textures from the HDRP Demo Scene, to create a variety of sandy terrain. Particularly, a sand texture was edited to be whiter, in order to attempt to create difficulties for the Computer Vision to identify a white drone against the white sandy background.

## Runtime Importers

During runtime, 3D Models can be imported by the user through the StreamingAssets folder into the application and instantiated into the scene. This is achieved using the following assets:

### GLTFUtility v0.7 and JsonDotNet

[GLTFUtility](https://github.com/Siccity/GLTFUtility) (together with [JsonDotNet](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)) is used to import .glTF files from the StreamingAssets folder into the application. Read more on the [README in the folder](../unity/drone/Assets/GLTFUtility/README.md) or on the GitHub repository itself.

A modification was made in the [Importer.cs](../unity/drone/Assets/GLTFUtility/Scripts/Importer.cs) file to skip the importing of the materials. Instead, after the object is imported and instantiated, a custom material is applied to the object. This material's colour can also be changed with the [Flexible Color Picker](#flexible-color-picker-v250) as discussed above.

### Runtime OBJ Importer v2.02

[Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547) is used to import .obj files from the StreamingAssets folder into the application. Read more on the [README in the folder](../unity/drone/Assets/OBJImport/README.HTML).

A custom material is also applied to the object after import, which similarly can be changed with the [Flexible Color Picker](#flexible-color-picker-v250).