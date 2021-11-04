using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureQualityLoader : DropDownLoader
{
    protected override void Awake()
    {
        dropdownName = "textureQuality";
        base.Awake();
    }

    protected override void HandleValue()
    {
        base.HandleValue();
        manager.SetTextureQuality(value, false, true);
    }

    protected override void Reset()
    {
        base.Reset();
        if(QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
        {
            dropdown.SetValueWithoutNotify(0);
        }
        else
        {
            if (QualitySettings.realtimeReflectionProbes)
            {
                dropdown.SetValueWithoutNotify(2);
            }
            else
            {
                dropdown.SetValueWithoutNotify(1);
            }
        }
    }
}
