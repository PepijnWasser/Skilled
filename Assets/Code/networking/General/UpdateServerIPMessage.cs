using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateServerIPMessage : ISerializable
{
    public string IP;
    public int port;

    public void Deserialize(Packet pPacket)
    {
        IP = pPacket.ReadString();
        port = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(IP);
        pPacket.Write(port);
    }

    public UpdateServerIPMessage()
    {

    }

    public UpdateServerIPMessage(string _IP, int _port)
    {
        IP = _IP;
        port = _port;
    }
}
