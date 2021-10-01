using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Focusable : MonoBehaviour
{
    public CinemachineVirtualCamera displayCam;

    public bool isFocused;
    protected GameObject player;

    Interactable interactable;

    protected virtual void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (player != null)
        {
            TestFocus();
        }
    }

    void TestFocus()
    {
        if (isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && interactable.lookingAtTarget)
            {
                Focus();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeFocus();
            }
        }
    }

    public void Focus()
    {
        isFocused = true;
        player.GetComponent<MeshRenderer>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerRotation>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        displayCam.Priority = 12;
    }

    public void DeFocus()
    {
        isFocused = false;
        player.GetComponent<MeshRenderer>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerRotation>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        displayCam.Priority = 10;
    }
}
