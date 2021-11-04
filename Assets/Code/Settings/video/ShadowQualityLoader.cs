using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowQualityLoader : DropDownLoader
{
    protected override void Awake()
    {
        dropdownName = "shadowQuality";
        base.Awake();
    }

    protected override void HandleValue()
    {
        base.HandleValue();
        manager.SetShadowQuality(value, false, true);
    }

    protected override void Reset()
    {
        base.Reset();
        if(QualitySettings.shadowCascades == 0)
        {
            dropdown.SetValueWithoutNotify(0);
        }
        else if(QualitySettings.shadowCascades == 2)
        {
            dropdown.SetValueWithoutNotify(1);
        }
        else if(QualitySettings.shadowCascades == 4)
        {
            dropdown.SetValueWithoutNotify(2);
        }
    }
}
