using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LocalHostClient : MonoBehaviour
{
    float secondCounter = 0;

    [SerializeField] private string _server = "localhost";
    [SerializeField] private int _port = 55555;

    [HideInInspector] public TcpClient client;

    public bool ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(_server, _port);
            Debug.Log("Connected to server.");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Could not connect to server:");
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

                    Heartbeat heartbeat = new Heartbeat();
                    SendObject(heartbeat);
                }
            }

            catch (Exception e)
            {
                //for quicker testing, we reconnect if something goes wrong.
                Debug.Log(e.Message);
                client.Close();
            }
            
        }

    }

    private void SendObject(ISerializable pOutObject)
    {
        try
        {
            Debug.Log("sending packet" + pOutObject);
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
