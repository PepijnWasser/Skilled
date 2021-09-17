using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LocalHostClient : MonoBehaviour
{
    float secondCounter = 0;

    [HideInInspector] public string playerName;
    [HideInInspector] public TcpClient tcpClient;
    [HideInInspector] public UdpClient udpClient;

    private static LocalHostClient _instance;

    public static LocalHostClient Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LocalHostClient>();
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

            IPEndPoint ipEndPoint = new IPEndPoint(address, _port);
            udpClient = new UdpClient(ipEndPoint);

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

    public void SendObjectThroughUDP(USerializable pOutObject)
    {
        try
        {
            UDPPacket outPacket = new UDPPacket();
            outPacket.Write(pOutObject);

            udpClient.Send(outPacket.GetBytes(), outPacket.GetBytes().Length);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            udpClient.Close();
        }
    }
}
