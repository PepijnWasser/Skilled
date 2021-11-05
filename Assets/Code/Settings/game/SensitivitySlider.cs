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
        GameSettingsManager.settingsReset += ResetSetting;
       
        SetVisualToSaved();
        manager.SetSensitivity(PlayerInfo.sensitivity);
    }

    private void OnDestroy()
    {
        GameSettingsManager.settingsReset -= ResetSetting;
    }

    //called when page is loaded
    public override void ResetSetting()
    {
        SetVisualToSaved();
    }

    //sets setting data to saved data
    void SetVisualToSaved()
    {
        int sensitivity = PlayerPrefs.GetInt("sensitivity");
        slider.SetValueWithoutNotify(sensitivity);
        text.text = sensitivity.ToString();
    }

    public void SetSensitivity(float sensitivity)
    {
        int sensitivityRounded = (int)sensitivity;
        text.text = sensitivityRounded.ToString();
        manager.SetSensitivity(sensitivityRounded);
    }
}
