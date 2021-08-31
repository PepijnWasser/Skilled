using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/**
 * The main ChatLobbyClient where you will have to do most of your work.
 * 
 * @author J.C. Wichman
 */
public class LocalHostClient : MonoBehaviour
{
    float secondCounter = 0;

    [SerializeField] private string _server = "localhost";
    [SerializeField] private int _port = 55555;

    private TcpClient _client;

    public void connectToServer()
    {
        try
        {
            _client = new TcpClient();
            _client.Connect(_server, _port);
            Debug.Log("Connected to server.");
        }
        catch (Exception e)
        {
            Debug.Log("Could not connect to server:");
            Debug.Log(e.Message);
        }
    }

    /*
    private void Update()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter > 2)
        {
            secondCounter = 0;

            Heartbeat heartbeat = new Heartbeat();
            SendObject(heartbeat);
        }

        try
        {
            if (_client.Available > 0)
            {
                //we are still communicating with strings at this point, this has to be replaced with either packet or object communication
                byte[] inBytes = StreamUtil.Read(_client.GetStream());
                Packet inPacket = new Packet(inBytes);

                var tempOBJ = inPacket.ReadObject();

                /*
                if (tempOBJ is ChatMessage)
                {
                    ChatMessage message = tempOBJ as ChatMessage;
                    HandleChatMessage(message);
                }

                else if (tempOBJ is MakeAgentMessage)
                {
                    MakeAgentMessage message = tempOBJ as MakeAgentMessage;
                    HandleMakeAgentMessage(message);
                }
                else if (tempOBJ is MoveAgentMessage)
                {
                    MoveAgentMessage message = tempOBJ as MoveAgentMessage;
                    HandleMoveAgentMessage(message);
                }
                else if (tempOBJ is RemoveClientMessage)
                {
                    RemoveClientMessage message = tempOBJ as RemoveClientMessage;
                    HandleRemoveAgentMessage(message);
                }
                else
                {
                    Debug.Log("something else");
                }
                
            }
        }
        catch (Exception e)
        {
            //for quicker testing, we reconnect if something goes wrong.
           // Debug.Log(e.Message);
            //_client.Close();
           // connectToServer();
        }
    }
*/
    private void SendObject(ISerializable pOutObject)
    {
        try
        {
            Debug.Log("sending packet" + pOutObject);
            Packet outPacket = new Packet();
            outPacket.Write(pOutObject);

            StreamUtil.Write(_client.GetStream(), outPacket.GetBytes());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            _client.Close();
            connectToServer();
        }
    }
}
