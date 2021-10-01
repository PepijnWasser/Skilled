using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorItem : EnergyItem
{
    public GameObject door;
    public Vector3 basePos;

    protected virtual void Start()
    {
        basePos = door.transform.position;
        base.Start();
    }

    protected override void TurnOn()
    {
        base.TurnOn();
        door.transform.position = basePos + new Vector3(0, 1, 0);
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        door.transform.position = basePos;
    }
}
