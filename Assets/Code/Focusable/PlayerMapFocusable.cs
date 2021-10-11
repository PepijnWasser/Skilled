using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapFocusable : Focusable
{
    public MapCamera mapCam;

    protected override void Start()
    {
        base.Start();
        //player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }

    protected override void Update()
    {
        base.Update();
        if (isFocused)
        {
            mapCam.PanCamera();
        }
    }
}
