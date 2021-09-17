using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOwnerMessage : ISerializable
{
    public bool isOwner;

    public void Deserialize(TCPPacket pPacket)
    {
        isOwner = pPacket.ReadBool(); 
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(isOwner);
    }

    public ServerOwnerMessage()
    {

    }

    public ServerOwnerMessage(bool _isOwner)
    {
        isOwner = _isOwner;
    }
}
