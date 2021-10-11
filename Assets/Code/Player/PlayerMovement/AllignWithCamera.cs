using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AllignWithCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    private void Update()
    {
        Vector3 newRot = new Vector3(cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
        this.transform.rotation = Quaternion.Euler(newRot);
    }
}
