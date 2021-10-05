using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStationHealthRequest : ISerializable
{
    public int stationHealth;

    public void Deserialize(TCPPacket pPacket)
    {
        stationHealth = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(stationHealth);
    }

    public UpdateStationHealthRequest()
    {

    }

    public UpdateStationHealthRequest(int _stationHealth)
    {
        stationHealth = _stationHealth;
    }
}
