using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : SettingsTab
{
    public List<GameSetting> gameSettings;

    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;

    public delegate void Reset();
    public static event Reset settingsReset;

    int sensitivity = 0;

    protected override void OnEnable()
    {
        foreach(GameSetting setting in gameSettings)
        {
            setting.ResetSetting();
        }

        sensitivity = 0;
    }

    public void SetSensitivity(int _sensitivity)
    {
        sensitivity = _sensitivity;
        changeCreated?.Invoke(this);
    }

    public override void RestoreDefaults()
    {
        PlayerPrefs.SetInt("sensitivity", 300);

        settingsReset?.Invoke();
        changeCreated?.Invoke(this);
    }

    public override void SaveSettings()
    {
        PlayerInfo.sensitivity = sensitivity;
        PlayerPrefs.SetInt("sensitivity", sensitivity);
    }
}
