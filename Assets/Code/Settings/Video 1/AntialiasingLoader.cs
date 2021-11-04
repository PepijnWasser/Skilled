using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntialiasingLoader : DropDownLoader 
{
    protected override void Awake()
    {
        dropdownName = "antiAliasing";
        base.Awake();
    }

    protected override void HandleValue()
    {
        base.HandleValue();
        manager.SetAntialiasing(value, true);
    }

    protected override void Reset()
    {
        base.Reset();
        int val = QualitySettings.antiAliasing;
        if (val == 0)
        {
            dropdown.SetValueWithoutNotify(0);
        }
        else if (val == 2)
        {
            dropdown.SetValueWithoutNotify(1);
        }
        else if (val == 4)
        {
            dropdown.SetValueWithoutNotify(2);
        }
        else if (val == 8)
        {
            dropdown.SetValueWithoutNotify(3);
        }
    }
}
