using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public int totalEnergy;
    public int availibleEnergy;

    public delegate void EnergyChanged(int newAmount);
    public static event EnergyChanged energyChanged;

    private void Start()
    {
        availibleEnergy = totalEnergy;
    }

    public void AddEnergy()
    {
        availibleEnergy += 1;
        energyChanged?.Invoke(availibleEnergy);

    }

    public void RemoveEnergy()
    {
        availibleEnergy -= 1;
        energyChanged?.Invoke(availibleEnergy);
    }
}
