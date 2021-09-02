using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestColorChangeMessage : ISerializable
{
    public int sideToChangeTo;

    public void Deserialize(Packet pPacket)
    {
        sideToChangeTo = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(sideToChangeTo);
    }

    public RequestColorChangeMessage()
    {

    }

    public RequestColorChangeMessage(int _sideToChangeTo)
    {
        sideToChangeTo = _sideToChangeTo;
    }
}
