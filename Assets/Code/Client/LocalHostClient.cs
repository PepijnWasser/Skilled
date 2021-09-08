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
    [HideInInspector] public TcpClient client;


    public bool ConnectToServer(System.Net.IPAddress address, int _port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(address, _port);
            Debug.Log("Connected to server on port " + _port);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public bool ConnectToServer(string address, int _port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(address, _port);
            Debug.Log("Connected to server on port " + _port);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }


    private void Update()
    {
        if(client != null && client.Connected)
        {
            try
            {
                secondCounter += Time.deltaTime;
                if (secondCounter > 2)
                {
                    secondCounter = 0;

                    HeartBeat request = new HeartBeat();
                    SendObject(request);
                }
            }

            catch (Exception e)
            {
                Debug.Log(e.Message);
                client.Close();
            }
            
        }
    }

    public void SendObject(ISerializable pOutObject)
    {
        try
        {
            Packet outPacket = new Packet();
            outPacket.Write(pOutObject);

            StreamUtil.Write(client.GetStream(), outPacket.GetBytes());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            client.Close();
        }
    }
}
