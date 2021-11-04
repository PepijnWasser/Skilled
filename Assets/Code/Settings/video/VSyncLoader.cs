using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSyncLoader : DropDownLoader
{
    protected override void Awake()
    {
        dropdownName = "VSync";
        base.Awake();
    }

    protected override void HandleValue()
    {
        base.HandleValue();
        if(value == 0)
        {
            manager.SetVsync(1, true);
            dropdown.SetValueWithoutNotify(1);
        }
        else
        {
            manager.SetVsync(0, true);
            dropdown.SetValueWithoutNotify(0);
        }

    }

    protected override void Reset()
    {
        base.Reset();

        int val = QualitySettings.vSyncCount;
        if(val == 0)
        {
            dropdown.SetValueWithoutNotify(1);
        }
        else if( val == 1)
        {
            dropdown.SetValueWithoutNotify(0);
        }
    }
}
