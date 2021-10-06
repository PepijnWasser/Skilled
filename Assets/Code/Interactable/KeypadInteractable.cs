using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadInteractable : Interactable
{
    Keypad keypad;

    protected override void Start()
    {
        base.Start();
        keypad = GetComponent<Keypad>();
    }


    protected override void OnHit(RaycastHit hit)
    {
        if(hit.transform.gameObject == body && keypad.playerIsUsing == false)
        {
            if (keypad.isFocused == false)
            {
                base.OnHit(hit);
            }
        }
    }
}
