using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusMessage : ISerializable
{
    public bool muted;

    public void Deserialize(TCPPacket pPacket)
    {
        muted = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(muted);
    }

    public PlayerStatusMessage()
    {

    }

    public PlayerStatusMessage(bool _muted)
    {
        muted = _muted;
    }
}
