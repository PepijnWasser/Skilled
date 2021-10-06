using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : Focusable
{
    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;

    public int keypadID = 0;

    public delegate void KeypadUsed(int ID, bool InUse);
    public static event KeypadUsed keypadUsed;

    public bool playerIsUsing = false;

    protected override void Awake()
    {
        base.Awake();
        GameState.updateKeypadStatus += UpdateIsInUse;
    }

    protected override void Start()
    {
        base.Start();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        keypadInteractable = GetComponent<KeypadInteractable>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameState.updateKeypadStatus -= UpdateIsInUse;
    }

    protected override void TestFocus()
    {
        if (isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && keypadInteractable.lookingAtTarget)
            {
                Focus();
                playerIsUsing = true;
                keypadUsed?.Invoke(keypadID, playerIsUsing);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeFocus();
                playerIsUsing = false;
                keypadUsed?.Invoke(keypadID, playerIsUsing);
            }
        }
    }
    void UpdateIsInUse(UpdateKeypadStatusMessage message)
    {
        if (message.keypadID == keypadID)
        {
            playerIsUsing = message.isInUse;
        }
    }

}
