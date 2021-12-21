using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMuteResponse : ISerializable
{
    public int playerID;
    public bool muted;

    public void Deserialize(TCPPacket pPacket)
    {
        playerID = pPacket.ReadInt();
        muted = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerID);
        pPacket.Write(muted);
    }

    public SwitchMuteResponse()
    {

    }

    public SwitchMuteResponse(int _playerID, bool _muted)
    {
        playerID = _playerID;
        muted = _muted;
    }
}
