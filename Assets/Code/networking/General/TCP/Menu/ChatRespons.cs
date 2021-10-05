using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRespons : ISerializable
{
    public string chatMessage;
    public string sender = "";
    public int hourSend = 0;
    public int minuteSend = 0;
    public int secondSend = 0;

    public void Deserialize(TCPPacket pPacket)
    {
        chatMessage = pPacket.ReadString();
        sender = pPacket.ReadString();
        hourSend = pPacket.ReadInt();
        minuteSend = pPacket.ReadInt();
        secondSend = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(chatMessage);
        pPacket.Write(sender);
        pPacket.Write(hourSend);
        pPacket.Write(minuteSend);
        pPacket.Write(secondSend);
    }

    public ChatRespons()
    {

    }

    public ChatRespons(string _chatMessage, string _sender, int _hourSend, int _minuteSend, int _secondSend)
    {
        chatMessage = _chatMessage;
        sender = _sender;
        hourSend = _hourSend;
        minuteSend = _minuteSend;
        secondSend = _secondSend;
    }
}
