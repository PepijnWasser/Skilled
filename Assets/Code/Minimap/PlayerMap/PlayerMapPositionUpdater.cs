using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapPositionUpdater : MapPositionUpdater
{
    private void Awake()
    {
        GameState.updatePlayerCamPosition += SetCamPosition;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnDestroy()
    {
        GameState.updatePlayerCamPosition -= SetCamPosition;
    }

    void SetCamPosition(Vector3 newPos, float newZoom)
    {
        mapCamera.transform.position = newPos;
        mapCamera.camera.orthographicSize = newZoom;
    }
}
