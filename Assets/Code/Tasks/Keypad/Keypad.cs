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

    public override void Focus()
    {
        base.Focus();
        playerIsUsing = true;
        keypadUsed?.Invoke(keypadID, playerIsUsing);
    }

    public override void DeFocus()
    {
        base.DeFocus();
        playerIsUsing = false;
        keypadUsed?.Invoke(keypadID, playerIsUsing);
    }
    protected override void TestFocus()
    {
        if (isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.E) && keypadInteractable.lookingAtTarget)
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
    void UpdateIsInUse(UpdateKeypadStatusMessage message)
    {
        if (message.keypadID == keypadID)
        {
            playerIsUsing = message.isInUse;
        }
    }

}
