using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnergyUserStatusResponse : ISerializable
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

    public UpdateEnergyUserStatusResponse()
    {

    }

    public UpdateEnergyUserStatusResponse(int _id, bool _on)
    {
        id = _id;
        on = _on;
    }
}
