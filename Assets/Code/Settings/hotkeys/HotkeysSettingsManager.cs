using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotkeysSettingsManager : SettingsTab
{
    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;

    public List<KeybindSetter> keybindSetters;

    private void Awake()
    {
        InputManager.changeCreated += SetHasChanges;
    }

    protected override void OnEnable()
    {

    }

    private void OnDisable()
    {
        InputManager.ResetChanges();
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
