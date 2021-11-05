using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VideoSetting : MonoBehaviour
{
    public Dropdown dropdown;
    public VideoSettingsManager manager;

    public string dropdownName;

    protected virtual void Awake()
    {
        SetVisualsToSavedValues();
    }

    public virtual void ResetSetting()
    {
        SetVisualsToSavedValues();
    }

    protected abstract void SetVisualsToSavedValues();
}
