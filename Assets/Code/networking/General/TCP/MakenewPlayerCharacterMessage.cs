using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakenewPlayerCharacterMessage : ISerializable
{
    public bool isPlayer = true;
    public int playerID;
    public Vector3 characterPosition;
    public string playerName;
    public void Deserialize(TCPPacket pPacket)
    {
        isPlayer = pPacket.ReadBool();
        playerID = pPacket.ReadInt();
        characterPosition = pPacket.ReadVector3();
        playerName = pPacket.ReadString();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(isPlayer);
        pPacket.Write(playerID);
        pPacket.Write(characterPosition);
        pPacket.Write(playerName);
    }

    public MakenewPlayerCharacterMessage()
    {

    }

    public MakenewPlayerCharacterMessage(bool _isPlayer, Vector3 _position, int _playerID, string _playerName)
    {
        isPlayer = _isPlayer;
        characterPosition = _position;
        playerID = _playerID;
        playerName = _playerName;
    }
}
