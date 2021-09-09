using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColorRequest : ISerializable
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

    public UpdateColorRequest()
    {

    }

    public UpdateColorRequest(int _sideToChangeTo)
    {
        sideToChangeTo = _sideToChangeTo;
    }
}
