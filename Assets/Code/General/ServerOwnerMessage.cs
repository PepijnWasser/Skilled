using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOwnerMessage : ISerializable
{
    public bool isOwner;

    public void Deserialize(Packet pPacket)
    {
        isOwner = pPacket.ReadBool(); 
    }

    public void Serialize(Packet pPacket)
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
