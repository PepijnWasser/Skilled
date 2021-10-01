using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyItem : MonoBehaviour
{
    EnergyManager energyManager;
    protected bool on = false;

    public Image image;

    public Color onColor;
    public Color offColor;

    protected virtual void Start()
    {
        energyManager = GameObject.FindObjectOfType<EnergyManager>();
        TurnOff();
    }

    public  void switchEnergy()
    {
        if (!on)
        {
            if(energyManager.availibleEnergy > 0)
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

        image.color = onColor;
    }

    protected virtual void TurnOff()
    {
        on = false;
        energyManager.AddEnergy();

        image.color = offColor;
    }
}
