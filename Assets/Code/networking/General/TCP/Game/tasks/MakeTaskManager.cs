using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTaskManager : ISerializable
{
    public bool playerIsLeader;

    public void Deserialize(TCPPacket pPacket)
    {
        playerIsLeader = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerIsLeader);
    }

    public MakeTaskManager()
    {

    }

    public MakeTaskManager(bool _playerIsLeader)
    {
        playerIsLeader = _playerIsLeader;
    }
}
