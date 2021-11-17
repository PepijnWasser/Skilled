using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchableRoom : Switchable
{

    public List<Switchable> switchables = new List<Switchable>();

    public override void TurnOn()
    {
        foreach(Switchable switchable in switchables)
        {
            switchable.TurnOn();
        }
    }

    public override void TurnOff()
    {
        foreach (Switchable switchable in switchables)
        {
            switchable.TurnOff();
        }
    }
}
