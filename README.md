# drone
dsta project

## tasks completed
### blender
for modelling of the environment to use in unity
- [x] created scene, ground
- [x] created obstacles
> - buildings
> - trees
- [x] attached obstacles to ground using particle system (hair)
> - able to choose how many obstacles to appear  
> - able to randomise the properties of the obstacles:
> > - scale
> > - location
> > - rotation
> > - "weightedness" - how often each obstacle appears
- [x] added 2 more types of trees, more realistic
- [x] used Count for the randomisation

### unity
for controls and simulation
- [x] imported scene from blender
- [x] added drones into scene
- [x] added 2 cameras
> - 1 for world view
> - 1 for view from drone above
- [x] added script to control drone below
> with WASD movement
- [x] added rigid body collisions between drone and obstacles
- [x] update to 2020.3.30f1
> - was accidentally on older 2019.4.10f1
- [x] formatted to naming and layout conventions
> - renamed class KeyboardController
- [x] used Public Variables (Speed, KeyCode) for KeyboardController
- [x] attached KeyboardController to both drones
> - arrow keys for Drone A
> - WASD for Drone B
- [x] reloaded new scene and make Prefab for the scene
- [x] added Package [Live Capture](https://docs.unity3d.com/Packages/com.unity.live-capture@2.0/manual/index.html)
- [x] set up Live Capture
> - created Take Recorder
> - created Virtual Camera Actor
> - created Virtual Camera Device
- [ ] connect VLC to Companion App Server
> - VLC was unable to connect
- [ ] troubleshoot RTSP connection
> - tried connecting using [SharpRTSP Client](https://github.com/ngraziano/SharpRTSP)
> > - able to connect to test RTSP server
> > - issues when connected to Unity's Companion App Server

## to-do
### blender
- [ ] create more obstacles
> birds, branches etc.
- [ ] improve current obstacles
> e.g. make trees more detailed, more realistic, branches and leaves
- [ ] ability to finetune randomisation
> possible to enforce minimum gap between obstacles?

### unity
- [ ] output camera feed from drone
> RTSP from camera


## others
- [model-view-controller](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)
- potential issue when drone goes under an obstacle, blocked from vision?
- maximum height of obstacles - can they reach height of drone above?
> current height is set at 55m