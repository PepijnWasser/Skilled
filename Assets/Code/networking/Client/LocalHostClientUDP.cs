using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class LocalHostClientUDP : MonoBehaviour
{
    [HideInInspector] public UdpClient client = new UdpClient();

    ServerConnectionData serverInfo;


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

    private void Start()
    {
        serverInfo = GameObject.FindObjectOfType<ServerConnectionData>();

        int i = 0;
        bool finishedInitialization = false;

        while (finishedInitialization == false && i < 20)
        {
            try
            {
                client = new UdpClient(20700 + i);
                GameObject.FindObjectOfType<PlayerInfo>().udpSendPort = 20700 + i;
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
            IPEndPoint RemoteIP = new IPEndPoint(serverInfo.ip, serverInfo.udpPort);
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
