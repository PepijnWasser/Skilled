using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserLight : EnergyUser
{
    public Light light;

    protected override void TurnOn()
    {
        base.TurnOn();
        light.enabled = true;
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        light.enabled = false;
    }
}
