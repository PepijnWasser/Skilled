using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteractable : Interactable
{
    public bool lookingAtTarget;
    Terminal terminal;

    public List<GameObject> body;

    protected override void Start()
    {
        base.Start();
        terminal = GetComponent<Terminal>();
        range = terminal.playerRange;
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
        if (body.Contains(hit.transform.gameObject))
        {
            base.OnHit(hit);
            lookingAtTarget = true;
        }
    }
}
