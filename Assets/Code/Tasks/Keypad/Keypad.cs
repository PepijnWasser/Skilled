using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : MonoBehaviour
{
    public bool isFocused;
    public float playerRange;

    public string name;

    public CinemachineVirtualCamera TaskCam;

    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;

    GameObject player;

    protected virtual void Start()
    {
        GameManager.playerMade += SetPlayer;

        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        keypadInteractable = GetComponent<KeypadInteractable>();
    }

    protected virtual void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
    }

    private void Update()
    {
        if(player != null)
        {
            TestFocus();
        }
    }

    void TestFocus()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange && isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && keypadInteractable.lookingAtTarget)
            {
                Focus();
            }
        }
        if (isFocused)
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
        Cursor.lockState = CursorLockMode.Confined;
        TaskCam.Priority = 12;
    }

    public void DeFocus()
    {
        isFocused = false;
        player.GetComponent<MeshRenderer>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        TaskCam.Priority = 10;
        keypadCodeEnterer.DisplayWelcomeMessage();
    }

    void SetPlayer(GameObject _player)
    {
        player = _player;
    }

}
