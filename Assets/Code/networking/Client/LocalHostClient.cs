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
            client = new TcpClient();
            var result = client.ConnectAsync(address, _port).Wait(1000);
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
