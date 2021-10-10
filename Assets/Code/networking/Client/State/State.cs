using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public abstract class State : MonoBehaviour
{
    //communication methods
    protected LocalHostClientTCP tcpClientNetwork;
    protected LocalHostClientUDP udpClientNetwork;

    //info for server / client
    protected PlayerInfo playerInfo;
    protected ServerConnectionData serverInfo;

    //general variables
    protected float timeOutTime = 5f;
    protected float lastHeartbeat = 5f;

    //udp client to receive on
    protected UdpClient client = new UdpClient();


    protected virtual void Awake()
    {
        tcpClientNetwork = GameObject.FindObjectOfType<LocalHostClientTCP>();
        udpClientNetwork = GameObject.FindObjectOfType<LocalHostClientUDP>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        serverInfo = GameObject.FindObjectOfType<ServerConnectionData>();
    }


    protected virtual void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteIP);

        UDPPacket packet = new UDPPacket(received);
        var TempOBJ = packet.ReadObject();

        client.BeginReceive(new AsyncCallback(recv), null);
    }

    //handle incoming tcp data
    protected virtual void Update()
    {      
        try
        {
            if (tcpClientNetwork != null && tcpClientNetwork.tcpClient.Connected && tcpClientNetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientNetwork.tcpClient.GetStream());
                TCPPacket inPacket = new TCPPacket(inBytes);

                var tempOBJ = inPacket.ReadObject();
                if(tempOBJ is HeartBeat)
                {
                    RefreshHeartbeat();
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (tcpClientNetwork.tcpClient.Connected)
            {
                tcpClientNetwork.tcpClient.Close();
            }
        }     
    }

    //refreshes server heartbeat
    protected virtual void RefreshHeartbeat()
    {
        lastHeartbeat = timeOutTime;
    }
    
    //checks if the server is still alive
    protected bool CheckHeartbeat()
    {
        lastHeartbeat -= Time.deltaTime;
        if (lastHeartbeat < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
