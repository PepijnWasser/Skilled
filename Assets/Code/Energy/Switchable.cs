using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwitchableRoom))]
public abstract class Switchable : MonoBehaviour
{
    public abstract void TurnOn();

    public abstract void TurnOff();
}
