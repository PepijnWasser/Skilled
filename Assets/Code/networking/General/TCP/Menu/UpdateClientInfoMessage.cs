using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class UpdateClientInfoMessage : ISerializable
{
    public int receivePort;
    public int sendPort;
    public IPAddress ip;

    public void Deserialize(TCPPacket pPacket)
    {
        receivePort = pPacket.ReadInt();
        sendPort = pPacket.ReadInt();
        ip = pPacket.ReadIP();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(receivePort);
        pPacket.Write(sendPort);
        pPacket.Write(ip);
    }

    public UpdateClientInfoMessage()
    {

    }

    public UpdateClientInfoMessage(IPAddress _ip, int _sendPort, int _receivePort)
    {
        ip = _ip;
        sendPort = _sendPort;
        receivePort = _receivePort;
    }
}
