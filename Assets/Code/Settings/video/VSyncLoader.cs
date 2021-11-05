using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSyncLoader : VideoSetting
{
    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("VSync");
        dropdown.SetValueWithoutNotify(value);
    }
}
