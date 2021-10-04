using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInteractable : Interactable
{
    EnergyInterface energyManager;

    protected override void Start()
    {
        base.Start();
        energyManager = GetComponent<EnergyInterface>();
    }


    protected override void OnHit(RaycastHit hit)
    {
        if (hit.transform.gameObject == body)
        {
            if (energyManager.isFocused == false)
            {
                base.OnHit(hit);
            }
        }
    }
}
