using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : Focusable
{
    public string name;

    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;

    private void Awake()
    {
        GameManager.playerMade += SetPlayer;
    }

    protected virtual void Start()
    {
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        keypadInteractable = GetComponent<KeypadInteractable>();
    }

    private void OnDestroy()
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
        if (isFocused == false)
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

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }
}
