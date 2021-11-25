using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyText : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
        EnergyManager.energyChanged += SetEnergyText;
        SetEnergyText(GameObject.FindObjectOfType<EnergyManager>().availibleEnergy);
    }

    private void OnDestroy()
    {
        EnergyManager.energyChanged += SetEnergyText;

    }

    void SetEnergyText(int amount)
    {
        text.text = amount.ToString();
    }
}
