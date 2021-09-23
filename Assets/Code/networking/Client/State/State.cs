using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public abstract class State : MonoBehaviour
{
    protected LocalHostClientTCP tcpClientNetwork;
    protected LocalHostClientUDP udpClientNetwork;

    protected float timeOutTime = 5f;
    protected float lastHeartbeat = 5f;

    protected UdpClient client = new UdpClient();


    protected virtual void Awake()
    {
        tcpClientNetwork = GameObject.FindObjectOfType<LocalHostClientTCP>().GetComponent<LocalHostClientTCP>();
        udpClientNetwork = GameObject.FindObjectOfType<LocalHostClientUDP>().GetComponent<LocalHostClientUDP>();
    }


    protected virtual void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteIP);

        UDPPacket packet = new UDPPacket(received);
        var TempOBJ = packet.ReadObject();

        client.BeginReceive(new AsyncCallback(recv), null);
    }

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
                    HandleHeartbeat();
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

    protected virtual void HandleHeartbeat()
    {
        lastHeartbeat = timeOutTime;
    }
    
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
