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

    public bool ConnectToServer(System.Net.IPAddress address, int _port)
    {
        try
        {
            tcpClient = new TcpClient();

            //IPEndPoint ipEndPoint = new IPEndPoint(address, _port);
            //udpClient = new UdpClient(ipEndPoint);

            bool result = tcpClient.ConnectAsync(address, _port).Wait(1000);
            if (result)
            {
                Debug.Log("Connected to server on port " + _port);
                return true;
            }
            else
            {
                Debug.Log("failed to connect");
                return false;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
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
