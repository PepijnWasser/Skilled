using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRequest : ISerializable
{
    public string chatMessage;

    public void Deserialize(TCPPacket pPacket)
    {
        chatMessage = pPacket.ReadString();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(chatMessage);
    }

    public ChatRequest()
    {

    }

    public ChatRequest(string _chatMessage)
    {
        chatMessage = _chatMessage;
    }
}
