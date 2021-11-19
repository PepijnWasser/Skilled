using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCodeUpdateResponse : ISerializable
{
    public int id;
    public string message;

    public void Deserialize(TCPPacket pPacket)
    { 
        id = pPacket.ReadInt();
        message = pPacket.ReadString();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(id);
        pPacket.Write(message);
    }

    public KeypadCodeUpdateResponse()
    {

    }

    public KeypadCodeUpdateResponse(int _id, string _message)
    {
        id = _id;
        message = _message;
    }
}
