using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerPrefabManager : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    public Camera skyboxCamera;
    public GameObject nose;

    public PlayerMovement playerMovementScript;
    public PlayerRotation playerRotationScript;



    public void DisablePlayerActivity()
    {
        playerMovementScript.enabled = false;
        playerRotationScript.enabled = false;

        Destroy(camera.gameObject);
        Destroy(skyboxCamera.gameObject);
    }

    public void setNoseRotation(Vector3 degrees)
    {
        nose.transform.rotation = Quaternion.Euler(degrees);
    }
}
