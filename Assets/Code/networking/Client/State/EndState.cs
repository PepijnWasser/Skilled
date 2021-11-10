using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndState : State
{
    EndView endview;
    MySceneManager sceneManager;

    protected override void Awake()
    {
        base.Awake();
        endview = GetComponent<EndView>();
        SendEndScreenLoadedMessage();
        Cursor.lockState = CursorLockMode.Confined;
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
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
                else if(tempOBJ is JoinRoomMessage)
                {
                    JoinRoomMessage message = tempOBJ as JoinRoomMessage;
                    HandleJoinRoomMessage(message);
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

    void SendEndScreenLoadedMessage()
    {
        SceneLoadedMessage message = new SceneLoadedMessage(SceneLoadedMessage.scenes.endScreen);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    public void SendLoadLobbyMessage()
    {
        JoinRoomRequest request = new JoinRoomRequest(JoinRoomRequest.rooms.lobby);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    void HandleTasksCompleted(TaskscompletedMessage message)
    {
        endview.SetTasksCompleted(message.amountOfTasksCompleted);
    }

    void HandleJoinRoomMessage(JoinRoomMessage message)
    {
        if(message.roomToJoin == JoinRoomMessage.rooms.lobby)
        {
            sceneManager.LoadScene("LobbyScene");
        }
        else
        {
            Debug.Log("unknown room");
        }
    }
}
