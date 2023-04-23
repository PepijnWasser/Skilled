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
    public  SkinnedMeshRenderer characterRenderer;

    public AllignWithCamera allignScript;

    public GameObject player;
    public Text iconText;

    public GameObject lookTarget;

    public PlayerMovement playerMovementScript;
    public PlayerRotation playerRotationScript;

    public PlayerPositionUpdater playerPositionUpdater;

    public PlayerAnimations playerAnimator;

    public Text playerName;


    public void DisablePlayerActivity()
    {
        playerMovementScript.enabled = false;
        playerRotationScript.enabled = false;
        characterRenderer.enabled = true;

        Destroy(playerCam.gameObject);
        Destroy(vCamera.gameObject);
        Destroy(skyboxCamera.gameObject);
        Destroy(playerPositionUpdater);
        allignScript.enabled = false;

        this.gameObject.tag = "MainPlayer";
    }
}
