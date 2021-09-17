using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateColorRespons : ISerializable
{
    string colorS;
    public int playerID;
    public MyClient.colors color;

    public void Serialize(TCPPacket packet)
    {
        packet.Write(colorS);
        packet.Write(playerID);
    }

    public void Deserialize(TCPPacket packet)
    {
        colorS = packet.ReadString();
        color = (MyClient.colors)Enum.Parse(typeof(MyClient.colors), colorS);
        playerID = packet.ReadInt();
    }

    public UpdateColorRespons()
    {

    }

    public UpdateColorRespons(MyClient.colors _color, int _playerID)
    {
        colorS = _color.ToString();
        playerID = _playerID;
    }
}