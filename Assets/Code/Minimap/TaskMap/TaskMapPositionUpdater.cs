using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMapPositionUpdater : PositionUpdater
{
    private void Awake()
    {
        GameState.updateTaskCamPosition += SetCamPosition;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        GameState.updateTaskCamPosition -= SetCamPosition;
    }

    void SetCamPosition(Vector3 newPos)
    {
        mapCamera.transform.position = newPos;
    }
}
