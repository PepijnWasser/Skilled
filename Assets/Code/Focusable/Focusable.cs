using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class Focusable : MonoBehaviour
{
    public CinemachineVirtualCamera displayCam;

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
            player.GetComponent<MeshRenderer>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerRotation>().enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            displayCam.Priority = 12;

            originalActionMap = InputManager.activeAction;
            InputManager.ToggleActionMap(InputManager.focusable);
        }
    }

    public virtual void DeFocus()
    {
        if (isFocused)
        {
            isFocused = false;
            player.GetComponent<MeshRenderer>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerRotation>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            displayCam.Priority = 10;

            InputManager.ToggleActionMap(originalActionMap);
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }
}
