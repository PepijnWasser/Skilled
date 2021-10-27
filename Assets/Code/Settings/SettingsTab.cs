using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingsTab : MonoBehaviour
{
    public bool hasChanges;

    public abstract void SaveSettings();


    private void Awake()
    {
        SettingsManager.disabled += RemoveChanges;
    }

    private void OnDisable()
    {
        SettingsManager.disabled -= RemoveChanges;
    }

    protected virtual void RemoveChanges()
    {
        hasChanges = false;
    }

    public abstract void RestoreDefaults();
}
