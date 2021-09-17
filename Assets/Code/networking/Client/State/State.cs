using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public abstract class State : MonoBehaviour
{
    protected LocalHostClient clientnetwork;

    protected float timeOutTime = 5f;
    protected float lastHeartbeat = 5f;

    
    protected virtual void Awake()
    {
        clientnetwork = GameObject.FindObjectOfType<LocalHostClient>().GetComponent<LocalHostClient>();
    }
    
    protected virtual void Update()
    {      
        try
        {
            if (clientnetwork != null && clientnetwork.tcpClient.Connected && clientnetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(clientnetwork.tcpClient.GetStream());
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
            if (clientnetwork.tcpClient.Connected)
            {
                clientnetwork.tcpClient.Close();
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
