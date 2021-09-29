using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverInteractable : Interactable
{
    public bool lookingAtTarget;
    TwoWayLever lever;

    public List<GameObject> body;

    protected override void Start()
    {
        base.Start();
        lever = GetComponent<TwoWayLever>();
        range = lever.playerRange;
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
