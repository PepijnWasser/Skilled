using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class UpdateClientInfoMessage : ISerializable
{
    public int port;
    public IPAddress ip;

    public void Deserialize(TCPPacket pPacket)
    {
        port = pPacket.ReadInt();
        ip = pPacket.ReadIP();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(port);
        pPacket.Write(ip);
    }

    public UpdateClientInfoMessage()
    {

    }

    public UpdateClientInfoMessage(IPAddress _ip, int _port)
    {
        ip = _ip;
        port = _port;
    }
}
