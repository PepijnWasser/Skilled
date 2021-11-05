using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowQualityLoader : VideoSetting
{
    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("shadowQuality");
        dropdown.SetValueWithoutNotify(value);
    }
}
