using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeLoader : VideoSetting
{
    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("fullScreenMode");
        dropdown.SetValueWithoutNotify(value);
    }
}
