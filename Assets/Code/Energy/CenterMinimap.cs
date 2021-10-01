using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMinimap : MonoBehaviour
{
    public Camera energyMinimapCamera;

    public void ResetCamera()
    {
        energyMinimapCamera.transform.position = new Vector3(0, 100, 0);
    }
}
