using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMinimap : MonoBehaviour
{
    public Camera mapCamera;

    public void ResetCamera()
    {
       mapCamera.transform.position = new Vector3(0, 100, 0);
    }
}
