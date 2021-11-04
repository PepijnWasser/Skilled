using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OveralQualityLoader : DropDownLoader
{
    protected override void Awake()
    {
        base.Awake();
        dropdownName = "overallQuality";
        int value = PlayerPrefs.GetInt(dropdownName);
        if (value == 3)
        {
            List<string> newOptions = new List<string>();
            newOptions.Add("custom");
            dropdown.AddOptions(newOptions);
        }
        dropdown.SetValueWithoutNotify(value);
    }

    protected override void Reset()
    {
        base.Reset();
        //see value in videosettingsmanager
        dropdown.SetValueWithoutNotify(2);
    }
}
