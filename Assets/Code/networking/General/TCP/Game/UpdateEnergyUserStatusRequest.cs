using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnergyUserStatusRequest : ISerializable
{
    public int id;
    public bool on;

    public void Deserialize(TCPPacket pPacket)
    {
        id = pPacket.ReadInt();
        on = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(id);
        pPacket.Write(on);
    }

    public UpdateEnergyUserStatusRequest()
    {

    }

    public UpdateEnergyUserStatusRequest(int _id, bool _on)
    {
        id = _id;
        on = _on;
    }
}
