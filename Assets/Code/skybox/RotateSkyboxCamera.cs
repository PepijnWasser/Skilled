using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotateSkyboxCamera : MonoBehaviour
{
    public CinemachineVirtualCamera mainCam;
    public GameObject player;

    float counter = 0;

    public float rotationSpeed;
    public Vector3 rotationAxis;

    private void Update()
    {
        counter += Time.deltaTime;

        float xRot = mainCam.transform.rotation.eulerAngles.x;

        float yRot = player.transform.rotation.eulerAngles.y;
        float zRot = player.transform.rotation.eulerAngles.z;

        Quaternion rotationToMatchCamera = Quaternion.Euler(xRot, yRot, zRot);

        Quaternion rotationQuaternion = Quaternion.AngleAxis(counter * rotationSpeed, rotationAxis);
        transform.rotation = rotationQuaternion * rotationToMatchCamera;
    }
}
