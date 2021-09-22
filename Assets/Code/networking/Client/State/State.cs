using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public abstract class State : MonoBehaviour
{
    protected LocalHostClientTCP tcpClientnetwork;
    protected LocalHostClientUDP udpClientnetwork;

    protected float timeOutTime = 5f;
    protected float lastHeartbeat = 5f;

    
    protected virtual void Awake()
    {
        tcpClientnetwork = GameObject.FindObjectOfType<LocalHostClientTCP>().GetComponent<LocalHostClientTCP>();
        udpClientnetwork = GameObject.FindObjectOfType<LocalHostClientUDP>().GetComponent<LocalHostClientUDP>();
    }
    
    protected virtual void Update()
    {      
        try
        {
            if (tcpClientnetwork != null && tcpClientnetwork.tcpClient.Connected && tcpClientnetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientnetwork.tcpClient.GetStream());
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
            if (tcpClientnetwork.tcpClient.Connected)
            {
                tcpClientnetwork.tcpClient.Close();
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
