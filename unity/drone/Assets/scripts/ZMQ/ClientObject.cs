using System.Collections.Concurrent;
using System.Threading;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;

public class NetMqListener
{
    private readonly Thread _listenerWorker;

    private bool _listenerCancelled;

    public delegate void MessageDelegate(string message);

    private readonly MessageDelegate _messageDelegate;

    private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

    private void ListenerWork()
    {
        AsyncIO.ForceDotNet.Force();
        using (var subSocket = new PullSocket())
        {
            subSocket.Connect("tcp://10.255.252.63:5556");
            // subSocket.Subscribe("");
            while (!_listenerCancelled)
            {
                string msg;
                // msg = subSocket.ReceiveFrameString();
                if (!subSocket.TryReceiveFrameString(out msg)) continue;
                Debug.Log(msg);
                _messageQueue.Enqueue(msg);
            }
            subSocket.Close();
        }
        NetMQConfig.Cleanup();
    }

    public void Update()
    {
        while (!_messageQueue.IsEmpty)
        {
            string message;
            if (_messageQueue.TryDequeue(out message))
            {
                _messageDelegate(message);
            }
            else
            {
                break;
            }
        }
    }

    public NetMqListener(MessageDelegate messageDelegate)
    {
        _messageDelegate = messageDelegate;
        _listenerWorker = new Thread(ListenerWork);
    }

    public void Start()
    {
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

    public void Stop()
    {
        _listenerCancelled = true;
        _listenerWorker.Join();
    }
}

public class ClientObject : MonoBehaviour
{
    public DirectVelocity Converter;
    private NetMqListener _netMqListener;

    private void HandleMessage(string message) // where the received messages will be processed
    {
        var inputs = message.Split(',');
        float vx = float.Parse(inputs[0]);
        float vy = float.Parse(inputs[1]);
        Converter.Convert(vx, vy);
        
        // Debug.Log(splittedStrings);
        // if (splittedStrings.Length != 3) return;
        // var x = float.Parse(splittedStrings[0]);
        // var y = float.Parse(splittedStrings[1]);
        // var z = float.Parse(splittedStrings[2]);
        // transform.position = new Vector3(x, y, z);

    }

    private void Start()
    {
        Debug.Log("starting zmq client");
        _netMqListener = new NetMqListener(HandleMessage);
        _netMqListener.Start();
        Debug.Log("started zmq client");
    }

    private void Update()
    {
        _netMqListener.Update();
    }

    private void OnDestroy()
    {
        _netMqListener.Stop();
    }
}