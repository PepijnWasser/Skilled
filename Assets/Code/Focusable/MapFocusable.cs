using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFocusable : Focusable
{
    public MapCamera mapCam;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
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
