using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColorRespons : ISerializable
{
    public string color = "red";
    public int playerID;

    public void Serialize(Packet packet)
    {
        packet.Write(color);
        packet.Write(playerID);
    }

    public void Deserialize(Packet packet)
    {
        color = packet.ReadString();
        playerID = packet.ReadInt();
    }

    public UpdateColorRespons()
    {

    }

    public UpdateColorRespons(MyClient.colors _color, int _playerID)
    {
        color = _color.ToString();
        playerID = _playerID;
    }
}