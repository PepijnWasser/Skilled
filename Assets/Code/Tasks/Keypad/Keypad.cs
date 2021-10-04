using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : Focusable
{
    public string name;

    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;


    protected virtual void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;

        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        keypadInteractable = GetComponent<KeypadInteractable>();
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
}
