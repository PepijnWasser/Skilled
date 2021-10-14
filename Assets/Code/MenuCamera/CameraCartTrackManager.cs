using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCartTrackManager : CartManager
{
    public CinemachineDollyCart cart;

    public float normalSpeed;

    public override void ResetCart()
    {
        cart.m_Position = 0;
        SetSpeed(normalSpeed);
    }

    public override void SetSpeed(float speed)
    {
        cart.m_Speed = speed;
    }

    public override void SetTrack(CinemachinePath path)
    {
        base.SetTrack(path);
        cart.m_Path = path;
    }
}
