using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmLight : Switchable
{
    bool on;
    public GameObject rotateAble;
    public Light[] lights;
    public int rotationSpeed;

    private void Awake()
    {
        on = true;
    }

    public override void TurnOff()
    {
        on = false;
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

    public override void TurnOn()
    {
        on = true;
        foreach(Light light in lights)
        {
            light.enabled = true;
        }
    }

    private void Update()
    {
        if (on)
        {
            rotateAble.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0), Space.Self);
        }
    }
}
