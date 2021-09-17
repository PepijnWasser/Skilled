using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerNameRequest : ISerializable
{
    public string playerName;

    public void Deserialize(TCPPacket pPacket)
    {
        playerName = pPacket.ReadString();    
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerName);
    }

    public UpdatePlayerNameRequest()
    {

    }

    public UpdatePlayerNameRequest(string _playerName)
    {
        playerName = _playerName;
    }
}
