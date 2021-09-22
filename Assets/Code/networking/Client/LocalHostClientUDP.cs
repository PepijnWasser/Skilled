using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LocalHostClientUDP : MonoBehaviour
{

    private static LocalHostClientUDP _instance;
    UdpClient client = new UdpClient();

    public static LocalHostClientUDP Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LocalHostClientUDP>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }



    public void SendObjectThroughUDP(USerializable pOutObject)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.15"), 33337);
        try
        {
            UDPPacket outPacket = new UDPPacket();
            outPacket.Write(pOutObject);

            client.Send(outPacket.GetBytes(), outPacket.GetBytes().Length, ipEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            client.Close();
        }        
    }
}
