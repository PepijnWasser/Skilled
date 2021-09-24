using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabManager : MonoBehaviour
{
    public Camera camera;
    public Camera skyboxCamera;

    public PlayerMovement playerMovementScript;
    public PlayerRotation playerRotationScript;

    public void DisablePlayerActivity()
    {
        playerMovementScript.enabled = false;
        playerRotationScript.enabled = false;

        Destroy(camera.gameObject);
        Destroy(skyboxCamera.gameObject);
    }
}
