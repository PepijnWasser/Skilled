using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInteractable : Interactable
{
    Focusable focusable;

    protected override void Start()
    {
        base.Start();
        focusable = GetComponent<Focusable>();
    }


    protected override void OnHit(RaycastHit hit)
    {
        if (hit.transform.gameObject == body)
        {
            if (focusable.isFocused == false)
            {
                base.OnHit(hit);
            }
        }
    }
}
