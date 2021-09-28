using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadInteractable : Interactable
{
    Keypad keypad;
    public GameObject body;
    float range;

    public bool lookingAtTarget;

    protected override void Start()
    {
        base.Start();
        keypad = GetComponent<Keypad>();
        range = keypad.playerRange;
    }

    protected override void Update()
    {
        lookingAtTarget = false;
        RaycastHit hit;

        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        if (dist < range)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                OnHit(hit);
            }
        }
    }

    protected override void OnHit(RaycastHit hit)
    {
        if(hit.transform.gameObject == body)
        {
            if (keypad.isFocused == false)
            {
                base.OnHit(hit);
                lookingAtTarget = true;
            }
        }
    }
}
