using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpRespons : ISerializable
{
    public string message;
    public string sender = "";
    public int hourSend = 0;
    public int minuteSend = 0;
    public int secondSend = 0;

    public void Deserialize(Packet pPacket)
    {
        message = pPacket.ReadString();
        sender = pPacket.ReadString();
        hourSend = pPacket.ReadInt();
        minuteSend = pPacket.ReadInt();
        secondSend = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(message);
        pPacket.Write(sender);
        pPacket.Write(hourSend);
        pPacket.Write(minuteSend);
        pPacket.Write(secondSend);
    }

    public HelpRespons()
    {

    }

    public HelpRespons(string _chatMessage, string _sender, int _hourSend, int _minuteSend, int _secondSend)
    {
        message = _chatMessage;
        sender = _sender;
        hourSend = _hourSend;
        minuteSend = _minuteSend;
        secondSend = _secondSend;
    }
}
