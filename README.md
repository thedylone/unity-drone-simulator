# drone versus drone

drone

## Contents

- [blender](#blender)
    - [installation](#installation)
    - [setup](#setup)
- [unity](#unity)
    - [installation](#installation-1)
    - [setup](#setup-1)
    - [system requirements](#system-requirements)

## blender

easy modelling of the scenes for unity. with procedural generation.

### installation

- install Blender v3 and later
    - earlier versions not guaranteed

### setup

TBC

## unity

engine simulation with RTSP streaming capabilities

based on [FFmpegOut](https://github.com/keijiro/FFmpegOut), with changes to FFmpeg to output RTSP stream

### installation

- install Unity 2020.3.30f1 or later
- this repository already includes FFmpegOut and the Binary package

### setup

- open Scenes folder and select Template
    - Template comes with a Main Camera pre-configured
- import scene from blender or create entirely within unity
    - for physics e.g. collision, select all elements in scene from Blender and add:
        - Rigidbody
        - Mesh Collider
        - Is Kinematic if necessary
- (optional) attach controller scripts onto models
- if not using Template or other pre-configured scenes:
    - create a camera (or use existing camera)
    - follow steps in [FFmpegOut](https://github.com/keijiro/FFmpegOut/blob/master/README.md)
        - add Camera Capture component
        - (optional) add Frame Rate Controller component
- does not come with RTSP server. consider using [rtsp-simple-server](https://github.com/aler9/rtsp-simple-server) to create the server.
- simply connect to the RTSP server with a RTSP client e.g. VLC

### system requirements

for FFmpegOut:

- Unity 2018.3 or later
- Windows: Direct3D 11
- macOS: Metal
- Linux: Vulkan

FFmpegOut only supports desktop platforms.

FFmpegOut works not only on the legacy rendering paths (forward/deferred) but also on the standard scriptable render pipelines (LWRP/HDRP).