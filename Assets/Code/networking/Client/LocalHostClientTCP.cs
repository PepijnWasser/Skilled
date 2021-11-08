using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LocalHostClientTCP : MonoBehaviour
{
    float secondCounter = 0;

    [HideInInspector] public TcpClient tcpClient;

    private static LocalHostClientTCP _instance;

    bool tryingToConnect = false;

    public static LocalHostClientTCP Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LocalHostClientTCP>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //tries to connect in 1 second
    //prints a message on fail
    public bool ConnectToServer(System.Net.IPAddress address, int _port)
    {
        if (tryingToConnect == false)
        {
            tryingToConnect = true;
            try
            {
                if(tcpClient == null)
                {
                    tcpClient = new TcpClient();
                }

                bool result = tcpClient.ConnectAsync(address, _port).Wait(100);
                if (result)
                {
                    Debug.Log("Connected to server on port " + _port);
                    tryingToConnect = false;
                    return true;
                }
                else
                {
                    Debug.Log("failed to connect");
                    tryingToConnect = false;
                    return false;
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                tryingToConnect = false;
                return false;
            }
        }
        else
        {
            return false;
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
