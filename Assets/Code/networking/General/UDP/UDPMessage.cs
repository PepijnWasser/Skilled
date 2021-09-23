using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPMessage : USerializable
{
    public string message;

    public void Deserialize(UDPPacket pPacket)
    {
        message = pPacket.ReadString();
    }

    public void Serialize(UDPPacket pPacket)
    {
        pPacket.Write(message);
    }

    public UDPMessage()
    {

    }

    public UDPMessage(string _message)
    {
        message = _message;
    }
}
