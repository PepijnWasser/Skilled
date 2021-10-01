using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUser : MonoBehaviour
{
    EnergyManager energyManager;
    protected bool on = false;

    public GameObject minimapIcon;

    public Color onColor;
    public Color offColor;

    protected virtual void Start()
    {
        energyManager = GameObject.FindObjectOfType<EnergyManager>();
        TurnOff();
    }

    public void switchEnergy()
    {
        Debug.Log("hit");
        if (!on)
        {
            if (energyManager.availibleEnergy > 0)
            {
                TurnOn();
            }
        }
        else
        {
            TurnOff();
        }
    }

    protected virtual void TurnOn()
    {
        on = true;
        energyManager.RemoveEnergy();

        minimapIcon.GetComponent<MeshRenderer>().material.color = onColor;
    }

    protected virtual void TurnOff()
    {
        on = false;
        energyManager.AddEnergy();

        minimapIcon.GetComponent<MeshRenderer>().material.color = offColor;
    }
}
