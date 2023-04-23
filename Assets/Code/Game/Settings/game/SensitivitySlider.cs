using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : GameSetting
{
    public Text text;
    public GameSettingsManager manager;
    public Slider slider;

    private void Awake()
    {
        GameSettingsManager.settingsReset += RestoreToDefault;
       
        SetVisualToSaved();
    }

    private void OnDestroy()
    {
        GameSettingsManager.settingsReset -= RestoreToDefault;
    }

    //called when page is loaded

    //sets setting data to saved data
    public override void SetVisualToSaved()
    {
        int sensitivity = PlayerPrefs.GetInt("sensitivity");
        slider.SetValueWithoutNotify(sensitivity);
        text.text = sensitivity.ToString();
    }

    public void SetSensitivity(float sensitivity)
    {
        int sensitivityRounded = (int)sensitivity;
        text.text = sensitivityRounded.ToString();
        manager.SetSensitivity(sensitivityRounded, false);
    }

    public override void RestoreToDefault()
    {
        int sensitivity = 300;
        slider.SetValueWithoutNotify(sensitivity);
        text.text = sensitivity.ToString();
    }
}
