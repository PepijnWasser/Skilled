using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchableRoom : Switchable
{
    public List<Switchable> onWhenPowered = new List<Switchable>();
    public List<Switchable> offWhenPowered = new List<Switchable>();

    private void Start()
    {
        TurnOff();
    }

    public override void TurnOn()
    {
        foreach(Switchable switchable in onWhenPowered)
        {
            switchable.TurnOn();
        }
        foreach (Switchable switchable in offWhenPowered)
        {
            switchable.TurnOff();
        }
    }

    public override void TurnOff()
    {
        foreach (Switchable switchable in onWhenPowered)
        {
            switchable.TurnOff();
        }
        foreach (Switchable switchable in offWhenPowered)
        {
            switchable.TurnOn();
        }
    }
}
