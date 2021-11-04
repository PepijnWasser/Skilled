using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownLoader : MonoBehaviour
{
    public Dropdown dropdown;
    public VideoSettingsManager manager;

    protected string dropdownName;

    protected int value;

    protected virtual void Awake()
    {
        VideoSettingsManager.settingsReset += Reset;

        value = PlayerPrefs.GetInt(dropdownName);
        HandleValue();
    }

    private void OnDestroy()
    {
        VideoSettingsManager.settingsReset -= Reset;
    }

    protected virtual void HandleValue()
    {
        dropdown.SetValueWithoutNotify(value);
    }

    protected virtual void Reset()
    {

    }
}
