using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCodeOutcomeResponse : ISerializable
{
    public int id;
    public bool correct;

    public void Deserialize(TCPPacket pPacket)
    {
        id = pPacket.ReadInt();
        correct = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(id);
        pPacket.Write(correct);
    }

    public KeypadCodeOutcomeResponse()
    {

    }

    public KeypadCodeOutcomeResponse(int _id, bool _correct)
    {
        id = _id;
        correct = _correct;
    }
}
