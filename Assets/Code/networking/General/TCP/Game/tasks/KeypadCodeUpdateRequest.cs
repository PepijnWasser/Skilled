using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCodeUpdateRequest : ISerializable
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

    public KeypadCodeUpdateRequest()
    {

    }

    public KeypadCodeUpdateRequest(int _id, string _message)
    {
        id = _id;
        message = _message;
    }
}
