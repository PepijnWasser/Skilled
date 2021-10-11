using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMapFocusable : Focusable
{
    public MapCamera cam;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    protected override void Update()
    {
        base.Update();
        if (isFocused)
        {
            cam.PanCamera();
        }
    }
}
