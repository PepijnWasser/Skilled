using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class LocalHostClientUDP : MonoBehaviour
{
    [HideInInspector] public UdpClient client = new UdpClient();

    private static LocalHostClientUDP _instance;

    public static LocalHostClientUDP Instance
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

    private void Start()
    {
        int i = 0;
        bool finishedInitialization = false;

        while (finishedInitialization == false && i < 20)
        {
            try
            {
                client = new UdpClient(20700 + i);
                PlayerInfo.udpSendPort = 20700 + i;
                finishedInitialization = true;
            }
            catch (Exception e)
            {
                e.ToString();
                i++;
            }
        }
    }

    //send a userializable to the server on the known port/IP
    public void SendObjectThroughUDP(USerializable pOutObject)
    {
        try
        {
            IPEndPoint RemoteIP = new IPEndPoint(ServerConnectionData.ip, ServerConnectionData.udpPort);
            UDPPacket packet = new UDPPacket();
            packet.Write(pOutObject);

            client.Send(packet.GetBytes(), packet.GetBytes().Length, RemoteIP);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
