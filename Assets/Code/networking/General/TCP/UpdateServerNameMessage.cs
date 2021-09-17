using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateServerNameMessage : ISerializable
{

    public string serverName;

    public void Serialize(TCPPacket packet)
    {
        packet.Write(serverName);
    }

    public void Deserialize(TCPPacket packet)
    {
        serverName = packet.ReadString();
    }

    public UpdateServerNameMessage()
    {

    }

    public UpdateServerNameMessage(string _serverName)
    {
        serverName = _serverName;
    }
}
