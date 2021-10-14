using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrackManager : MonoBehaviour
{
    public CameraCartTrackManager cameraCartManger;
    public TargetCartTrackManager targetCartManager;

    public List<CinemachinePath> cameraPaths;
    public List<CinemachinePath> targetPaths;

    public int currentPath;

    private void Start()
    {
        SetNewPath(GetRandomTrack());
    }

    void Update()
    {
        if (cameraCartManger.cart.m_Position >= 1)
        {
            SetNewPath(GetRandomTrack());
        }
    }

    int GetRandomTrack()
    {
        int newPath = Random.Range(0, cameraPaths.Count);
        return newPath;
    }

    void SetNewPath(int newID)
    {
        currentPath = newID;

        cameraCartManger.ResetCart();
        targetCartManager.ResetCart();

        GetRandomTrack();
        cameraCartManger.SetTrack(cameraPaths[newID]);
        targetCartManager.SetTrack(targetPaths[newID]);
    }
}
