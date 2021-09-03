using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage : ISerializable
{
    public string chatMessage;
    public string sender = "";
    public int hourSend = 0;
    public int minuteSend = 0;
    public int secondSend = 0;

    public void Deserialize(Packet pPacket)
    {
        chatMessage = pPacket.ReadString();
        sender = pPacket.ReadString();
        hourSend = pPacket.ReadInt();
        minuteSend = pPacket.ReadInt();
        secondSend = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(chatMessage);
        pPacket.Write(sender);
        pPacket.Write(hourSend);
        pPacket.Write(minuteSend);
        pPacket.Write(secondSend);

    }

    public ChatMessage()
    {

    }

    public ChatMessage(string _chatMessage)
    {
        chatMessage = _chatMessage;
    }

    public ChatMessage(string _chatMessage, string _sender, int _hourSend, int _minuteSend, int _secondSend)
    {
        chatMessage = _chatMessage;
        sender = _sender;
        hourSend = _hourSend;
        minuteSend = _minuteSend;
        secondSend = _secondSend;

    }
}
