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
> RTSP from camera - need to research more
- [ ] add script to control drone above
> take in separate inputs? (avoid WASD conflict)
- [ ] ?

## others
- [model-view-controller](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)
- potential issue when drone goes under an obstacle, blocked from vision?
- maximum height of obstacles - can they reach height of drone above?
> current height is set at 55m