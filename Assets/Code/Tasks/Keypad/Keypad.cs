using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : Focusable
{
    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;

    protected override void Start()
    {
        base.Start();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        keypadInteractable = GetComponent<KeypadInteractable>();
    }


    protected override void TestFocus()
    {
        if (isFocused == false && playerIsUsing == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && keypadInteractable.lookingAtTarget)
            {
                Focus();
                playerIsUsing = true;
            }
        }
        if (isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeFocus();
                playerIsUsing = false;
            }
        }
    }
}
