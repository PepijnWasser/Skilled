using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class Focusable : MonoBehaviour
{
    public CinemachineVirtualCamera displayCam;

    public Camera cameraParent;

    public bool isFocused;
    protected GameObject player;

    Interactable interactable;

    InputActionMap originalActionMap;

    protected virtual void Awake()
    {
        GameManager.playerMade += SetPlayer;
        InputManager.savedControls.Game.Interact.performed += _ => TestFocus();
        InputManager.savedControls.Focusable.Leave.performed += _ => DeFocus();
    }

    protected virtual void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    protected virtual void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
        InputManager.savedControls.Game.Interact.performed -= _ => TestFocus();
        InputManager.savedControls.Focusable.Leave.performed -= _ => DeFocus();
    }

    protected virtual void TestFocus()
    {
        if (player != null)
        {
            if (interactable.lookingAtTarget)
            {
                Focus();
            }
        }
    }

    public virtual void Focus()
    {
        if (!isFocused)
        {
            isFocused = true;
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerRotation>().enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            displayCam.Priority = 12;

            originalActionMap = InputManager.activeAction;
            InputManager.SetActiveActionMap(InputManager.focusable);

            cameraParent.gameObject.SetActive(true);
        }
    }

    public virtual void DeFocus()
    {
        if (isFocused)
        {
            isFocused = false;
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerRotation>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            displayCam.Priority = 10;

            InputManager.SetActiveActionMap(originalActionMap);

            cameraParent.gameObject.SetActive(false);
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }
}
