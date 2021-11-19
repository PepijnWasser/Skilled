using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCodeOutcomeRequest : ISerializable
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

    public KeypadCodeOutcomeRequest()
    {

    }

    public KeypadCodeOutcomeRequest(int _id, bool _correct)
    {
        id = _id;
        correct = _correct;
    }
}
