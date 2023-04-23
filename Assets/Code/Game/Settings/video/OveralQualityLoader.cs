using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OveralQualityLoader : VideoSetting
{
    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("overallQuality");
        if (value == 3)
        {
            List<string> newOptions = new List<string>();
            newOptions.Add("custom");
            dropdown.AddOptions(newOptions);
        }
        dropdown.SetValueWithoutNotify(value);
    }
}
