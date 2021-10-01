using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightItem : EnergyItem
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
