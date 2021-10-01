using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public int totalEnergy;
    public int availibleEnergy;

    public Text energyCount;

    private void Start()
    {
        availibleEnergy = totalEnergy;
    }

    public void AddEnergy()
    {
        availibleEnergy += 1;
        energyCount.text = availibleEnergy.ToString();
    }

    public void RemoveEnergy()
    {
        availibleEnergy -= 1;
        energyCount.text = availibleEnergy.ToString();
    }
}
