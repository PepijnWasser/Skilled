using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerPrefabManager : MonoBehaviour
{
    public Camera playerCam;
    public Camera skyboxCamera;
    public CinemachineVirtualCamera vCamera;

    public AllignWithCamera allignScript;

    public GameObject player;
    public Text iconText;

    public GameObject nose;

    public PlayerMovement playerMovementScript;
    public PlayerRotation playerRotationScript;



    public void DisablePlayerActivity()
    {
        playerMovementScript.enabled = false;
        playerRotationScript.enabled = false;

        Destroy(playerCam.gameObject);
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
