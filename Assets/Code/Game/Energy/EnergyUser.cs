using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUser : MonoBehaviour
{
    EnergyManager energyManager;
    public bool on = false;

    public GameObject minimapIcon;

    public Color onColor;
    public Color offColor;

    public int id;

    public delegate void EnergyChanged(int id, bool on);
    public static EnergyChanged energyChanged;

    private void Awake()
    {
        GameState.energyUserUpdated += SetStatus;
    }

    protected virtual void Start()
    {
        id = EnergySpawner.getNewID();
        energyManager = GameObject.FindObjectOfType<EnergyManager>();
        TurnOff();
    }

    private void OnDestroy()
    {
        GameState.energyUserUpdated -= SetStatus;
    }

    public void switchEnergy()
    {
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

        energyChanged?.Invoke(id, on);
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

    void SetStatus(int _id, bool _on)
    {
        if(_id == id)
        {
            if (_on)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }
    }
}
