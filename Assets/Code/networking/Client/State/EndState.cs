using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndState : State
{
    EndView endview;

    protected override void Awake()
    {
        base.Awake();
        endview = GetComponent<EndView>();
        SendGameLoadedMessage();
    }

    //handle tcp messages
    protected override void Update()
    {
        try
        {
            if (tcpClientNetwork != null && tcpClientNetwork.tcpClient.Connected && tcpClientNetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientNetwork.tcpClient.GetStream());
                TCPPacket inPacket = new TCPPacket(inBytes);

                var tempOBJ = inPacket.ReadObject();

                if (tempOBJ is TaskscompletedMessage)
                {
                    TaskscompletedMessage message = tempOBJ as TaskscompletedMessage;
                    HandleTasksCompleted(message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + e.Source);
            if (tcpClientNetwork.tcpClient.Connected)
            {
                tcpClientNetwork.tcpClient.Close();
            }
        }
    }

    void SendGameLoadedMessage()
    {
        SceneLoadedMessage message = new SceneLoadedMessage(SceneLoadedMessage.scenes.endScreen);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void HandleTasksCompleted(TaskscompletedMessage message)
    {
        endview.SetTasksCompleted(message.amountOfTasksCompleted);
    }
}
