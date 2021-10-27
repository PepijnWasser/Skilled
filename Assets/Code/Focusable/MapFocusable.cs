using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFocusable : Focusable
{
    public MapCamera mapCam;

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (isFocused)
        {
            mapCam.PanCamera();
            mapCam.ZoomCamera();
        }
    }
}
