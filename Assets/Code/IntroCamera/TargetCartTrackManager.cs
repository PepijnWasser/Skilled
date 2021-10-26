using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCartTrackManager : CartManager
{
    public CinemachineDollyCart cart;

    public float normalSpeed;  

    public override void ResetCart()
    {
        cart.m_Position = 0.01f;
        SetSpeed(normalSpeed);
    }

    public override void SetTrack(CinemachinePath path)
    {
        base.SetTrack(path);
        cart.m_Path = path;
    }

    public override void SetSpeed(float speed)
    {
        cart.m_Speed = speed;
    }
}