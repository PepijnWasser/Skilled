using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableLight : Switchable
{
    public GameObject emission;

    public override void TurnOn()
    {
        emission.SetActive(true);
    }

    public override void TurnOff()
    {

        emission.SetActive(false);
    }
}
