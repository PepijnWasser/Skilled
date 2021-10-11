using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerPrefabManager : MonoBehaviour
{
    public Camera camera;
    public Camera skyboxCamera;
    public CinemachineVirtualCamera vCamera;

    public AllignWithCamera allignScript;

    public GameObject player;
    public GameObject icon;

    public GameObject nose;

    public PlayerMovement playerMovementScript;
    public PlayerRotation playerRotationScript;



    public void DisablePlayerActivity()
    {
        playerMovementScript.enabled = false;
        playerRotationScript.enabled = false;

        Destroy(camera.gameObject);
        Destroy(vCamera.gameObject);
        Destroy(skyboxCamera.gameObject);
        allignScript.enabled = false;

        this.gameObject.tag = "MainPlayer";
    }

    public void setNoseRotation(Vector3 degrees)
    {
        nose.transform.rotation = Quaternion.Euler(degrees);
    }
}
