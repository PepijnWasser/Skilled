using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SwitchableLight : Switchable
{
    Light light;
    public Renderer renderer;
    public Material mat;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    public override void TurnOn()
    {
        light.enabled = true;
        renderer.material.EnableKeyword("_EMISSION");
    }

    public override void TurnOff()
    {
        light.enabled = false;
        renderer.material.DisableKeyword("_EMISSION");
    }
}
