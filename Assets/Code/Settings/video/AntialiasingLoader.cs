using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntialiasingLoader : VideoSetting 
{
    public override void ResetSetting()
    {
        base.ResetSetting();
    }

    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("antiAliasing");
        dropdown.SetValueWithoutNotify(value);
    }
}
