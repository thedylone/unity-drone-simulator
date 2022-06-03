# drone versus drone

Unity Drone Simulator is an appllcation which allows you to simulate the '**Eagle Concept**', streaming out the Camera feed of the 'Hunter' Drone and receiving inputs to move the 'Hunter' Drone. This project is meant to test the capabilities of a **Computer Vision Model** and **PID Controller Script**.

Unity's High Definition Render Pipeline (HDRP) is used in this project to enhance the graphical fidelity, enabling the Simulator to be as similar to reality, thus testing the Computer Vision in near-realistic environments.

![Menu](images/Menu.png)

---

## Contents

1. [Eagle Concept](#eagle-concept)
2. [Quickstart](#quickstart)
3. [Settings and Configurations](#settings-and-configurations)
    - [RTSP Settings](#rtsp-settings)
    - [ZeroMQ Settings](#zeromq-settings)
    - [Drone Settings](#drone-settings)
4. [Runtime](#runtime)
    - [Test Case Menu](#test-case-menu)
    - [Waypoint Menu](#waypoint-menu)
    - [Circle Menu](#circle-menu)
    - [Comparison](#comparison)
5. [External Assets](#external-assets)

---

## Eagle Concept

A 'Hunter' Drone is flying above the 'Target' Drone, with its camera facing vertically down. The height between the 2 drones is the Separation Distance of the drones. The Hunter Drone's height is fixed at `55m`, while adjusting the Separation Distance will move the Target Drone's vertical position. For example, with a Separation Distance of `20m`, the Target Drone will be placed at `35m`.

![EagleDiagram](images/eagle.gif)

In this application, the user is looking from the Hunter Drone's Camera's Point of View, and is able to move the Target Drone with Keyboard Controls (WASD).

---

## Quickstart

Follow the below steps to use the Application together with a **Computer Vision** model and **Controller Script**.

1. Download the [latest release]().
> Alternatively, clone this repository and open the Unity Project (Unity 2020.3.30f1). You can run it in Unity Editor or build the Application yourself.
2. Launch the Application, as well as a RTSP Server, e.g. [rtsp-simple-server](/rtsp%20simple%20server/).
> **⚠️Warning!⚠️** Ensure that a server is running before starting the RTSP stream, else the Application will crash.
3. Configure Settings. Important Settings to note are:
    - RTSP output path
    - ZeroMQ listener path
    > Set the ZeroMQ listener path to be the output path of the **Controller Script**  
4. Click on **Start** to load into the Scene. RTSP and ZeroMQ will automatically start up.
> To confirm, the **rtsp-simple-server** will indicate that a session is publishing to the path provided in the settings.

Start up the **Computer Vision** and Controller Script.

1. If running on another device, ensure the devices are able to connect to each other, e.g. change firewall settings.
2. For the **Computer Vision**, consume the RTSP stream via the RTSP output path as set and shown in the Application's Settings.
> To confirm, the **rtsp-simple-server** will indicate that a session is reading from the path provided in the settings.

Move the Target Drone with WASD, and observe that the **Computer Vision** is able to detect the drone. The **Controller Script** should then receive the detections, and subsequently send commands to move the Hunter Drone. The Hunter Drone, also the Application's POV, should be moving and following the Target Drone.

Press `Escape` or click on the gear icon in the top left corner to open up the Menu while running. The **Test Case Menu**, **Waypoint Menu** and **Circle Menu** will allow you to automatically move the Target Drone. For more details, refer to the [Runtime Section](#runtime).

---

## Settings and Configurations

Entering the Application will bring up the Main Menu. A Quick Glance sidebar is also displayed, allowing quick and easy access to critical settings. The Main Menu contains the following:

- Start
    > Loads Scene and automatically starts RTSP and ZeroMQ.
- Settings
    > Opens up Settings Page (see below).
- Fullscreen
    > Toggles Fullscreen (based on Windowed Mode's Aspect Ratio).
- Quit
    > Closes the application (stops running if in Editor).

The Settings Page will enable you to modify the below:

### RTSP Settings

The application is able to stream out the Hunter Drone's camera feed via RTSP using FFmpegOut. For more information on FFmpegOut, do refer to the [FFmpegOut README](/docs/FFmpegOut.md) or [the GitHub repository](https://github.com/keijiro/FFmpegOut).

> **⚠️Warning!⚠️** Ensure that a server is running before starting the RTSP stream. Consider using [rtsp-simple-server](https://github.com/aler9/rtsp-simple-server) to run the server, located at [/rtsp simple server](/rtsp%20simple%20server/). Refer to the [rtsp-simple-server README](/docs/rtsp-simple-server.md) for information on setting up.

<details><summary>Settings Guide</summary>

Adjust the **Port Number** and **URL** for the RTSP output path. By default, Port Number is `8554` and URL is `/drone`. The application will display a list of IP Address your device is connected to. This RTSP stream can then be accessed from another device by connecting to a listed IP Address.

> Ensure that the two devices are able to connect to each other, e.g. change firewall settings.

An additional **Delay** can be introduced into the RTSP stream by adjusting the slider in the Settings page. Select how much to delay the stream in milliseconds.
> Note that this will not account for the existing latency, meaning that with a base latency of `500ms`, adding `1000ms` delay will result in a combined delay of `1500ms`.

Click to toggle the **Timestamp**, which will show the time elapsed since the application was opened.
> Note that this Timestamp, located in the bottom left of the screen, will be visible in the RTSP stream. This is useful for measuring latency between the application and the receiving client.

If necessary, adjust the other parameters for the stream:

- **Width** (default 1920),
- **Height** (default 1080), 
- and **Frame Rate** (default 30).

</details>

---

### ZeroMQ Settings

The application is able to receive inputs via ZeroMQ, to move the Hunter Drone. The **Controller Script** should send in inputs as `Vx,Vy`. 
> ℹ️ `Vx` and `Vy` should be within the range from -1 to 1, which will then be modified by the Drone's set Max Speed.

<details><summary>Settings Guide</summary>

Enter the **Path** for the application to connect and listen to.

A **Delay** can also be introduced, meaning that inputs will only be executed a set fixed time after receiving from the **Controller Script**.
> Similar to the RTSP delay, this will not account for the existing latency.

</details>

---

### Drone Settings

Modify the Settings for both the Hunter Drone and the Target Drone.

<details><summary>Settings Guide</summary>

As mentioned in [Eagle Concept](#eagle-concept), **Separation Distance** will adjust the height of the Target Drone from the Hunter Drone, from a range of 1m to 50m.

The **Max Speed** of the Hunter Drone and the Target Drone can be adjusted. This will determine how fast the drones will travel. For example, with `Vx = 0.5` and **Max Speed** of `10m/s`, the drone will move at `5m/s`.

The 3D Model for the Target Drone can also be changed. This repository comes with the models of DJI Mavic and DJI Phantom. New models can also be imported by placing them into the `Assets/Resources/Drone Models` folder, as well as the `StreamingAssets/Drone Models` folder. 

During runtime, models can be imported through the `StreamingAssets/Drone Models` folder. Supported formats are:

- [obj](https://en.wikipedia.org/wiki/Wavefront_.obj_file)
- [glTF (includes .gltf and .glb)](https://en.wikipedia.org/wiki/GlTF)

The Refresh Button next to the Model Dropdown will reload all the files and import the models. For more info on the Runtime Importers, refer to the [Assets README on Runtime Importers](/docs/Assets.md#runtime-importers).

The Target Drone's **Material Colour** can also be edited using the Flexible Color Picker. For more info on the Flexible Color Picker, refer to the [Assets README on the Flexible Color Picker](/docs/Assets.md#flexible-color-picker-v250) or the [FlexibleColorPickerDoc.pdf](unity/drone/Assets/FlexibleColorPicker/FlexibleColorPickerDoc.pdf).

</details>

---

## Runtime

Upon pressing **Start**, the RTSP Stream and ZeroMQ Client will both be automatically started. The Target Drone can then be controlled with Keyboard Controls (WASD).

> **⚠️Warning!⚠️** Attempting to Start without a RTSP server will cause the Application to crash.

Press `Escape` on your keyboard or click on the gear icon in the top left corner to open the Menu. The following options are available in the Menu:

- [Test Case Menu](#test-case-menu)
- [Waypoint Menu](#waypoint-menu)
- [Circle Menu](#circle-menu)
- Return to Main Menu
    > Stops the RTSP Stream and ZeroMQ and will return to the Main Menu starting screen, where Settings can be changed.
- Reset Drone Positions
    > Moves the Hunter and Target Drones back to the starting position.
- Quit
    > Closes the application (stops running if in Editor).

The **Test Case Menu**, **Waypoint Menu**, and **Circle Menu** all allow the Target Drone to be moved automatically, not requiring manual input from the Keyboard.

### Test Case Menu

A Test Case is a recording of the Target Drone's Velocity normalised by `Max Speed`, automatically moving the Target Drone without requiring manual input. The Application will check if the Target Drone is within the boundaries of the Camera Screen. If the Target Drone remains in view of the Camera for the entire duration of the Test Case, it is deemed as a **Pass**, else it will be considered a **Fail**.

Both Recording and Replaying are possible. Test Cases are stored in the `StreamingAssets/testdata` folder. Test Cases can be manually imported into the folder. However, it is recommended to create a Test Case via the Recording function.

To save a Recording, input a custom file name or leave blank to auto-generate a new file name. Inputting an existing file name will overwrite the existing Test Case. Press the 'Save' button to start recording, and press the 'Save' button again to stop recording.

All Test Cases found are listed in the Test Case Menu. The 'Play' button next to each Test Case will Load the respective Test Case. Loading another Case while one is running will end the existing Case and start the new Case.

The 'Load All' button will successively load each Test Case, displaying the output of each Case whether it is a **Pass** or **Fail**.

---

### Waypoint Menu

A Waypoint is a more human-readable format to give directions to the Target Drone. The Waypoint will provide the `x` and `y` coordinates, as well as the `speed` for the Target Drone to move.

Both Recording and Replaying are possible. Waypoints are stored in the `StreamingAssets/waypoint` folder. Waypoints can be created via the Recording function. However, it is recommended to manually create a Waypoint, to ensure precision.

To save a Recording, input a custom file name or leave blank to auto-generate a new file name. Inputting an existing file name will overwrite the existing Waypoint. Press the 'Start' button to start recording, and press the 'Stop' button to stop recording.

All Waypoints found are listed in the Waypoint Menu. The 'Play' button next to each Waypoint will Load the respective Waypoint. Loading another Waypoint while one is running will end the existing Waypoint and start the new Waypoint.

---

### Circle Menu

Enabling the Circle function will allow the Target Drone to orbit around the initial position. The desired **Radius** and **Speed**, as well as **Direction** (Clockwise or Counter-Clockwise) can be changed in the Menu.

> ℹ Note that the Circling function is disabled while a Test Case or a Waypoint is being Loaded.

---

### Comparison

Below is a comparison among the 3 methods of automated movement:

| Test Case | Waypoint | Circle |
| --- | --- | --- |
| Saves Velocity | Saves Location and Speed | Cannot Save |
| Precise Recording | Inaccurate Recording | Cannot Save |
| Less readable | More Readable | N/A |
| Changes Velocity | Changes Velocity | Changes Position |
| Stops if exits screen | Loads entire file | Infinite duration |
| Affected by Max Speed | Independent, set in file | Set Speed in Menu |

---

## External Assets

Additional Asset Packages were used to assist in the creation of this project. More details can be found on the [Assets README](/docs/Assets.md).

- [Unity Terrain - HDRP Demo Scene](https://assetstore.unity.com/packages/3d/environments/unity-terrain-hdrp-demo-scene-213198)
- [EasyRoads3D](https://assetstore.unity.com/packages/3d/characters/easyroads3d-free-v3-987)
- [Flexible Color Picker](https://assetstore.unity.com/packages/tools/gui/flexible-color-picker-150497)
- [GLTFUtility](https://github.com/Siccity/GLTFUtility)
- [JsonDotNet](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)
- [Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/model)
- [Street Lights Pack](https://assetstore.unity.com/packages/3d/props/exterior/street-lights-pack-31644)
- [Yughues Free Sand Materials](https://assetstore.unity.com/packages/2d/textures-materials/floors/yughues-free-sand-materials-12964)