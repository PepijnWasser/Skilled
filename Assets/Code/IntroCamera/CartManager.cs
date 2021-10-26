using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class CartManager : MonoBehaviour
{
    protected CinemachinePath currentPath;

    public abstract void ResetCart();

    public virtual void SetTrack(CinemachinePath path)
    {
        currentPath = path;
    }

    public abstract void SetSpeed(float speed);
}
