# drone versus drone

drone

## Contents

- unity
    - installation
    - setup
    - system requirements

## unity

### installation

- install Unity 2021.2.14f1 or later
- packages required:
    - High Definition Render Pipeline (HDRP) (v12.1.5)
    - Terrain Tools (v4.0.3)
- assets:
    - EasyRoads3D Free v3
        - *optional, for creating of roads on terrain*
    - Street Lights Pack v1
        - *optional, for models for street lights*
    - Unity Terrain - HDRP Demo Scene
        - *optional, most assets are borrowed from here and already present in the repository*
    - Yughues Free Sand Materials
        - *optional, sand texture for terrain layer, some are already used and present in the scenes’ terrain*

### setup

- input manager
    - KeyboardController retrieves Input from axis “Keyboard Horizontal” and “Keyboard Vertical”
    - JoystickController retrieves Input from axis “Horizontal” and “Vertical”
    - Under Edit > Project Settings > Input Manager
        - Set “Horizontal” and “Vertical” axes to desired control buttons, and set “Type” to “Joystick Axis”
        - Rename to or create new axes “Keyboard Horizontal” and “Keyboard Vertical”, set desired control buttons, and set “Type” to “Key or Mouse Button”
- controls
    - Attach “Keyboard Controller” or “Joystick Controller” to GameObject to add movement controls
    - Set the desired Speed of the object
        - Applies only if Converter is not set
    - Enable Hover to allow the object to keep a set distance from the terrain regardless of the y-value of the terrain
        - Uses Raycast, and turns on gravity for the object
        - Ensures that the object will not clip through the terrain if the terrain exceeds the fixed height
        - Set the desired Hover Distance for the object to hover above the terrain
        - Set the Layer the object will hover over
            - This requires setting the terrain’s layer to the selected layer
            - To set the layer, in the Inspector at the top right, set the Layer property to desired layer
            - The number next to the layer is the number that should be input into the Controller’s Layer field
            - Raycast will then be affected by the set layer
    - Set the Converter if necessary
        - Use the Converter if more processing to the Inputs are required
        - Converter can convert Input to Velocity/Force e.g. DirectVelocity, VelocityConverter
        - Attach Converter by:
            - Adding the Converter script as a component to the object
            - Dragging the object into the Converter field
        - To change the type of Converter:
            - Open up the Controller script
            - Change “public DirectVelocity converter” and replace with “public [Converter Script Name] converter”
        - If Converter is not set, the Controller will simply add a Force based on the Speed set onto the object
- converter
    - Velocity Converter
        - Takes input and tilts object, adding force to reach desired velocity
        - Input should be between -1 and 1, and object’s velocity will be `Input * MaxSpeed`
        - Set MaxSpeed, and object’s velocity will be limited to -MaxSpeed to MaxSpeed
        - Set MaxTilt, and object’s rotation will be limited to -MaxTilt to MaxTilt
        - Set TiltSpeed to limit the object’s rotation speed
        - Forces are calculated taking air resistance into consideration
        - Using the formula `F = mg tan θ - kv²`, where `θ` is the angle of tilt, `kv²` is the drag, and taking `F = 0` when object is at MaxTilt and MaxSpeed
        - Force is applied to the object based on its tilt
        - The converter script will affect the rotation of the object, and the velocity is physically calculated
        - Tilt to achieve is calculated by `arctan(tan(MaxTilt) * v²)`
        - Current tilt of the object is calculated by `arctan((dv)² * k / mg)`, where `dv = Input * MaxSpeed * 2 - CurrentSpeed`, `k` is the drag coefficient as calculated in the formula above. The current tilt is limited by MaxTilt and TiltSpeed
    - Direct Velocity
        - Takes input and directly sets the object velocity
        - Input should be between -1 and 1, and object’s velocity will be `Input * MaxSpeed`
        - Set MaxSpeed, and object’s velocity will be limited to -MaxSpeed to MaxSpeed
        - Set MaxTilt, and object’s rotation will be limited to -MaxTilt to MaxTilt
        - Set Enable Tilt
            - Object will tilt if enabled
            - Tilt angle is calculated as in Velocity Converter
- terrain
    - Using Unity Terrain Tools to create terrain
    - Paint Terrain
        - Raise or Lower Terrain
            - Use different brushes to create terrain
            - Brushes from the HDRP Demo Scene add a variety of textures and brushes
        - Paint Texture
            - By default, the terrain’s texture will be the first layer in the Layer Palette
            - Select desired texture layer
            - Use Brush Mask Filters if needed to selectively paint over certain areas
            - To add layer, either Add Layer and select existing layer, or Create New Layer and add the Diffuse and Normal Map
    - Paint Trees
        - Edit Trees > Add Tree > Tree Prefab and select the tree
        - Paint the terrain with trees or use Mass Place Trees to fill the entire terrain with Trees
    - Paint Details
        - Edit Details >  Add Detail Mesh > Detail Prefab
        - Paint the terrain with details
        - The details’ shading may look incorrect
            - To fix, select the detail’s material and press Fix Diffusion Profile
- roads *(optional)*
    - Uses the EasyRoads3D asset to create
    - GameObject > 3D Object > EasyRoads3D > New Road Network
    - Add Road Object > Type: 1. Default Road > Add New Object
    - Use Shift + Click to add Markers to define where the road will be
    - Once done, Update Terrain > Build Terrain and Finalize
    - If Road texture does not render correctly due to HDRP, select the Road material and change the shader to HDRP/Lit
- lamps *(optional)*
    - Open up Lamps Prefabs and select the desired model
    - If Lamp texture does not render correctly due to HDRP, select the Lamp material and change the shader to HDRP/Lit
    - Add a Light to the Lamp and adjust the parameters
- volumes
    - HDRP volumes
    - Add the default sky Volume to your Scene to set up ambient lighting GameObject > Volume > Sky and Fog Global Volume
- lights
    - Create a directional light to represent the Sun
    - Adjust the properties to mimic the physical attributes of the Sun
        - Angular Diameter of 0.5
        - Enable Affect Physically Based Sky
        - Distance of 1.5e+08
        - Temperature of 5700K
        - Set Intensity to Preset or customise the Intensity
        - Enable Contact Shadows

### output

Unity is able to stream a camera’s output via RTSP. This allows the feed of 1 camera to be streamed and viewed elsewhere, such as on another computer, while Unity displays the feed of another camera. This was achieved using [FFmpegOut]([https://github.com/keijiro/FFmpegOut](https://github.com/keijiro/FFmpegOut)), with minor changes to the flags when running FFmpeg.

- RTSP flags: `-re -f rtsp -rtsp_transport tcp -muxdelay 0.1`

To setup:

- Create the Camera to output the stream
- Adjust the Camera properties as desired
- Attach the Camera Capture script as a component
- Set the output video width and height as desired
- Tick Enable RTSP to output a RTSP stream
    - If disabled, FFmpeg will save a recording instead
- Specify output path
    - RTSP e.g. `rtsp://127.0.0.1:8554/stream`
    - If RTSP disabled and path is blank, FFmpeg will save a recording to the root folder
- Using Preset H.264 NVIDIA or HEVC NVIDIA is recommended (requires NVIDIA GPU)
    - [https://docs.nvidia.com/video-technologies/video-codec-sdk/ffmpeg-with-nvidia-gpu/](https://docs.nvidia.com/video-technologies/video-codec-sdk/ffmpeg-with-nvidia-gpu/)
- *(Optional)* Attach the Frame Rate Controller script as a component

Upon playing, the FFmpeg will start up. If RTSP is enabled, a popup will appear warning you to make sure a RTSP server is open first. Press OK or OK and don’t remind again to proceed.

- If no RTSP server is running, it will not be able to output and Unity will crash!
- Cancelling or closing the popup will switch FFmpeg to record instead of stream

This does not come with a RTSP server, consider using [rtsp-simple-server](https://github.com/aler9/rtsp-simple-server) to run the server.

Simply connect to the RTSP server with any RTSP client, e.g. VLC.

### input

Unity is able to receive inputs from a Python script. For instance, inputs sent from Python to control the movement of the object can be received by Unity, and then converted into actions inside Unity. This was achieved using [Two-way communication between Python 3 and Unity (C#) - Y. T. Elashry]([https://github.com/Siliconifier/Python-Unity-Socket-Communication](https://github.com/Siliconifier/Python-Unity-Socket-Communication)).

To setup:

- Python
    - Place UdpComms.py into the folder
    - In the main python script, add the following lines of code:
    
    ```python
    import UdpComms as U
    
    """
    :param udpIP: Must be string e.g. "127.0.0.1"
    :param portTX: integer number e.g. 8000. Port to transmit from i.e From Python to other application
    :param portRX: integer number e.g. 8001. Port to receive on i.e. From other application to Python
    :param enableRX: When False you may only send from Python and not receive. If set to True a thread is created to enable receiving of data
    :param suppressWarnings: Stop printing warnings if not connected to other application
    """
    sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)
    
    while True:
        sock.SendData("some data") # Send this string to other application
        data = sock.ReadReceivedData() # read data
        if data != None: # if NEW data has been received since last ReadReceivedData function call
            print(data) # print new received data
    ```
    
    - Run the main python script
- Unity
    - Add UdpSocket.cs to a game object in a scene
    - Edit the script as needed
    - Alternatively, use the pre-edited script available in this project

The UdpSocket.cs in this project has a few modifications for the purpose of this project.

- Output to Python
    - The script gets the screen position of the target on the camera screen and sends it to the Python script
    - Set the target to be the desired target
    - Set the camera to be the camera the script will take the positions from
- Input from Python
    - The script will take inputs from Python, which are Vx and Vy, between -1 and 1
    - The inputs are sent into the Converter (see above) to move the object
    - Set the object to be the object to be moved by the Python script (object must have the converter script attached to it)

These can be modified under the `void Update()` function in the script. For instance, to disable sending of the output to the Python script, comment out or just delete the portion regarding the sending of data.

### system requirements

for FFmpegOut:

- Unity 2018.3 or later
- Windows: Direct3D 11
- macOS: Metal
- Linux: Vulkan

FFmpegOut only supports desktop platforms.

FFmpegOut works not only on the legacy rendering paths (forward/deferred) but also on the standard scriptable render pipelines (LWRP/HDRP).