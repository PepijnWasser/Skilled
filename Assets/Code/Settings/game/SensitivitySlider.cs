using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : GameSetting
{
    public Text text;
    public GameSettingsManager manager;
    public Slider slider;

    public delegate void Changes(int newSensitivity);
    public static event Changes changeCreated;

    private void Awake()
    {
        GameSettingsManager.settingsReset += ResetStat;
       
        GetValue();
        manager.SetSensitivity(PlayerInfo.sensitivity);
    }

    private void OnDestroy()
    {
        GameSettingsManager.settingsReset -= ResetStat;
    }

    public void SetSensitivity(float sensitivity)
    {
        int sensitivityRounded = (int)sensitivity;
        text.text = sensitivityRounded.ToString();
        manager.SetSensitivity(sensitivityRounded);
        changeCreated?.Invoke(sensitivityRounded);
    }

    private void ResetStat()
    {
        GetValue();
        changeCreated?.Invoke(PlayerInfo.sensitivity);
    }

    void GetValue()
    {
        int sensitivity = PlayerPrefs.GetInt("sensitivity");
        slider.SetValueWithoutNotify(sensitivity);
        text.text = sensitivity.ToString();
    }

    public override void ResetSetting()
    {
        GetValue();
    }
}
