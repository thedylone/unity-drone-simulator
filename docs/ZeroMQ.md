# ZeroMQ

[ZeroMQ](https://zeromq.org/) is used as the networking library to serve as the connection between the Controller Script and the Unity Application.

ZeroMQ is used in this Application since it is meant to replicate the Controller Script's connection to the DJI App, which uses ZeroMQ. An alternative is available via UDP.

Most of the files were sourced from [Unity-ZeroMQ-Example](https://github.com/valkjsaaa/Unity-ZeroMQ-Example).

## Modifications

To change the functionality of the ZeroMQ client, 

1. In the class `ClientObject`, change the function `HandleMessage`

2. Modify the code within `HandleMessage` to parse the message and then use the parsed inputs as you desire.