# RTSP Simple Server

[rtsp-simple-server](https://github.com/aler9/rtsp-simple-server) is provided in the repository to enable the Unity application to publish the RTSP stream.

In order for the server to listen to the **Port** set in the Unity Application, the configuration needs to be changed for the address of the RTSP Listener. By default, the Port is `:8554`.

Edit the `rtsp-simple-server.yml` file and set `rtspAddress: :8554` to be the desired Port.

Read more on changing the configuration on [the rtsp-simple-server README](https://github.com/aler9/rtsp-simple-server/blob/main/README.md#configuration)