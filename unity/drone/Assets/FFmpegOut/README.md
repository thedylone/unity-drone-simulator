# FFmpegOut

[FFmpegOut](https://github.com/keijiro/FFmpegOut) is a Unity plugin used to record and livestream the Unity Camera feed. Some modifications were made to the FFmpegOut Package to achieve streaming via RTSP.

## Setup

Setup is similar to the steps in the original package, so most of the instructions in [FFmpegOut README](https://github.com/keijiro/FFmpegOut#camera-capture-component) can be followed.

## Modifications

Several properties were changed to instead retrieve the value from the Global Settings static fields.
- <b>Width</b> (default 1920)
- <b>Height</b> (default 1080)
- <b>Frame Rate</b> (default 30)

An option to <b>Enable RTSP</b> was added. Turning on <b>Enable RTSP</b> will allow FFmpeg to stream out the camera feed via RTSP. Disabling RTSP instead will keep FFmpegOut's intended functionality to save a recording of the camera feed instead.

The <b>Path</b> property was also added. It retrieves the value from the Global Settings static fields. It determined by the <b>Port Number</b> and the <b>URL</b>, determined by `rtsp://localhost:<Port Number>/<URL>`. If the <b>Port Number</b> and <b>URL</b> are both left blank in the Settings, RTSP will be turned <b>off</b> and will save a recording instead.

It is recommended to use the <b>H.264 NVIDIA</b> preset, but will require a NVIDIA GPU. As it is highly optimised, there is minimal latency and can perform well at high frame rates and/or high resolutions.

## RTSP Streaming

RTSP streaming is enabled using the following flags:

 `-f rtsp -rtsp_transport udp -muxdelay 0.1`

 However, a RTSP server is still required for FFmpeg to stream to. <b>Attempting to run and stream via RTSP without a server will cause Unity to crash.</b>
 
 Ensure that a server is running before starting the RTSP stream. This repository does <b>not</b> come with a RTSP server, consider using [rtsp-simple-server](https://github.com/aler9/rtsp-simple-server) to run the server.

 Once the RTSP stream is running, the stream can be accessed by any RTSP client, e.g. VLC, FFplay, and connect to the stream via the input Path.

 The RTSP stream can also be accessed remotely on another device on the same network, by accessing `rtsp://<IP Address>:<Port Number>/<URL>`. Ensure that the two devices are able to connect to each other, e.g. firewall.

 ## Requirements

 Requirements are similar to those stated in the [FFmpegOut README](https://github.com/keijiro/FFmpegOut/blob/master/README.md#system-requirements).

 However, it is recommended to use Unity 2020.3.30f1 for the rest of the project, and have a NVIDIA GPU for the H.264 NVIDIA preset.