using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureQualityLoader : VideoSetting
{
    protected override void SetVisualsToSavedValues()
    {
        int value = PlayerPrefs.GetInt("textureQuality");
        dropdown.SetValueWithoutNotify(value);
    }
}
