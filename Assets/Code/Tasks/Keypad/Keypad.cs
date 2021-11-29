using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Keypad : Focusable
{
    KeypadCodeEnterer keypadCodeEnterer;
    KeypadInteractable keypadInteractable;

    [HideInInspector]
    public int keypadID = 0;

    public float resetTime;


    public delegate void KeypadUsed(int ID, bool InUse);
    public static event KeypadUsed keypadUsed;

    public bool playerIsUsing = false;


    float secondCounter = 0;

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

    protected void Update()
    {
        if(keypadCodeEnterer.message != keypadCodeEnterer.nameMessage)
        {
            if(playerIsUsing == false)
            {
                secondCounter += Time.deltaTime;
                if(secondCounter > resetTime)
                {
                    secondCounter = 0;
                    keypadCodeEnterer.DisplayWelcomeMessage();
                }
            }
            else
            {
                secondCounter = 0;
            }
        }
        else
        {
            secondCounter = 0;
        }
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
        if (player != null)
        {
            if (isFocused == false)
            {
                if (keypadInteractable.lookingAtTarget)
                {
                    Focus();
                }
            }
            else
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
