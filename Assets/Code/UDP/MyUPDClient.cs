using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MyUPDClient : MonoBehaviour
{
    float secondCounter;

    [HideInInspector] public UdpClient udpClient;
    IPEndPoint ipEndPoint;

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
        ConnectToServer(IPAddress.Parse("192.168.2.15"), 55555);
    }


    public void ConnectToServer(System.Net.IPAddress address, int _port)
    {
        try
        {
            ipEndPoint = new IPEndPoint(address, _port);
            udpClient = new UdpClient(ipEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

        }
    }

    private void Update()
    {
        if (udpClient != null)
        {
            try
            {
                secondCounter += Time.deltaTime;
                if (secondCounter > 2)
                {
                    secondCounter = 0;

                    UDPMessage request = new UDPMessage("hello");
                    SendObjectThroughUDP(request);
                }
            }

            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }
    }

    public void SendObjectThroughUDP(USerializable pOutObject)
    {
        try
        {
            UDPPacket outPacket = new UDPPacket();
            outPacket.Write(pOutObject);

            byte[] bytes = outPacket.GetBytes();
            udpClient.Send(bytes, bytes.Length, ipEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            udpClient.Close();
        }
    }
}
