using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColorRequest : ISerializable
{
    public int sideToChangeTo;

    public void Deserialize(TCPPacket pPacket)
    {
        sideToChangeTo = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(sideToChangeTo);
    }

    public UpdateColorRequest()
    {

    }

    public UpdateColorRequest(int _sideToChangeTo)
    {
        sideToChangeTo = _sideToChangeTo;
    }
}
