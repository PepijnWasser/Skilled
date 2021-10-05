using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class UpdateServerInfo : ISerializable
{
    public int udpPort;
    public int tcpPort;
    public IPAddress ip;
    public string owner;
    public bool isOwner;

    public void Deserialize(TCPPacket pPacket)
    {
        udpPort = pPacket.ReadInt();
        tcpPort = pPacket.ReadInt();
        ip = pPacket.ReadIP();
        owner = pPacket.ReadString();
        isOwner = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(udpPort);
        pPacket.Write(tcpPort);
        pPacket.Write(ip);
        pPacket.Write(owner);
        pPacket.Write(isOwner);
    }

    public UpdateServerInfo()
    {

    }

    public UpdateServerInfo(int _udpPort, int _tcpPort, IPAddress _ip, string _owner, bool _isOwner)
    {
        udpPort = _udpPort;
        tcpPort = _tcpPort;
        ip = _ip;
        owner = _owner;
        isOwner = _isOwner;
    }
}
