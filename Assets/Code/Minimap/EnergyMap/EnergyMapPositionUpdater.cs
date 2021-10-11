using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMapPositionUpdater : PositionUpdater
{
    private void Awake()
    {
        GameState.updateEnergyCamPosition += SetCamPosition;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        GameState.updateEnergyCamPosition -= SetCamPosition;
    }

    void SetCamPosition(Vector3 newPos)
    {
        //mapCamera.transform.position = newPos;
    }
}
