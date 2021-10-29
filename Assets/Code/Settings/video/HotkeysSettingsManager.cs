using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeysSettingsManager : SettingsTab
{
    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;

    private void Awake()
    {
        InputManager.changeCreated += SetHasChanges;
    }

    private void OnDestroy()
    {
        InputManager.changeCreated -= SetHasChanges;
    }

    void SetHasChanges()
    {
        hasChanges = true;
        changeCreated?.Invoke(this);
    }

    public override void SaveSettings()
    {
        if (hasChanges)
        {
            InputManager.SaveBindings();
        }
    }

    public override void RestoreDefaults()
    {
        InputManager.RestoreToDefault();
    }
}
