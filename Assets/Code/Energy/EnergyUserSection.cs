using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserSection : EnergyUser
{
    public List<SwitchableRoom> switchableRooms = new List<SwitchableRoom>();

    protected override void TurnOn()
    {
        base.TurnOn();
        foreach (SwitchableRoom room in switchableRooms)
        {
            room.TurnOn();
        }
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        foreach(SwitchableRoom room in switchableRooms)
        {
            room.TurnOff();
        }
    }

    public void AddRoom(SwitchableRoom room)
    {
        switchableRooms.Add(room);

        room.TurnOff();
        
    }
}
