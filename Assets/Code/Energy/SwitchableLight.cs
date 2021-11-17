using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SwitchableLight : Switchable
{
    Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    public override void TurnOn()
    {
        light.enabled = true;
    }

    public override void TurnOff()
    {
        light.enabled = false;
    }
}
