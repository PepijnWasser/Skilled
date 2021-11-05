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

    public delegate void Saved();
    public static event Saved settingsSaved;

    int sensitivity = 0;

    private void Awake()
    {
        SetSavedValues();
        sensitivity = PlayerPrefs.GetInt("sensitivity");
        PlayerInfo.sensitivity = sensitivity;
    }

    protected override void OnEnable()
    {
        foreach(GameSetting setting in gameSettings)
        {
            setting.SetVisualToSaved();
        }
    }

    private void OnDisable()
    {
        SetSavedValues();
    }

    void SetSavedValues()
    {
        SetSensitivity(PlayerPrefs.GetInt("sensitivity"), true);
    }

    public void SetSensitivity(int _sensitivity, bool startup)
    {
        sensitivity = _sensitivity;
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public override void RestoreDefaults()
    {
        sensitivity = 300;

        foreach(GameSetting setting in gameSettings)
        {
            setting.RestoreToDefault();
        }

        settingsReset?.Invoke();
        changeCreated?.Invoke(this);
    }

    public override void SaveSettings()
    {
        PlayerInfo.sensitivity = sensitivity;
        PlayerPrefs.SetInt("sensitivity", sensitivity);

        settingsSaved?.Invoke();
    }
}
