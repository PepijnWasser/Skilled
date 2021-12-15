using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public struct ConnectionFeedback
{
    public bool connected;

    public Exception error;

    public ConnectionFeedback(bool _connected, Exception _error)
    {
        this.connected = _connected;
        this.error = _error;
    }
}

public class LocalHostClientTCP : MonoBehaviour
{
    float secondCounter = 0;

    [HideInInspector] public TcpClient tcpClient;

    bool tryingToConnect = false;

    private static LocalHostClientTCP _instance;
    public static LocalHostClientTCP Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //tries to connect in 1 second
    //prints a message on fail
    public ConnectionFeedback ConnectToServer(System.Net.IPAddress address, int _port)
    {
        if (tryingToConnect == false)
        {
            tryingToConnect = true;
            try
            {
                if (tcpClient == null || tcpClient.Connected)
                {
                    tcpClient = new TcpClient();
                }

                if(_port == 0)
                {
                    tryingToConnect = false;
                    return new ConnectionFeedback(false, new Exception("Port cannot be zero"));
                }
                else
                {
                    bool result = tcpClient.ConnectAsync(address, _port).Wait(10);
                    if (result)
                    {
                        tryingToConnect = false;
                        return new ConnectionFeedback(true, null);
                    }
                    else
                    {
                        tryingToConnect = false;
                        return new ConnectionFeedback(false, new Exception("Failed to connect to given IP and port"));
                    }
                }
            }

            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
                tryingToConnect = false;
                return new ConnectionFeedback(false, new Exception(e.Message));

            }
        }
        else
        {
            return new ConnectionFeedback(false, new Exception("Already trying to connect"));
        }
    
    }


    private void Update()
    {
        if(tcpClient != null && tcpClient.Connected)
        {
            try
            {
                secondCounter += Time.deltaTime;
                if (secondCounter > 2)
                {
                    secondCounter = 0;

                    HeartBeat request = new HeartBeat();
                    SendObjectThroughTCP(request);
                }
            }

            catch (Exception e)
            {
                Debug.Log(e.Message);
                tcpClient.Close();
            }
            
        }
    }

    //sends a Iserializable to thr server through tcp
    public void SendObjectThroughTCP(ISerializable pOutObject)
    {
        try
        {
            TCPPacket outPacket = new TCPPacket();
            outPacket.Write(pOutObject);

            NetworkUtils.Write(tcpClient.GetStream(), outPacket.GetBytes());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            tcpClient.Close();
        }
    }
}
