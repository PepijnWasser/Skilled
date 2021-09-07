using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRequest : ISerializable
{
    public string chatMessage;

    public void Deserialize(Packet pPacket)
    {
        chatMessage = pPacket.ReadString();
    }

    public void Serialize(Packet pPacket)
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
