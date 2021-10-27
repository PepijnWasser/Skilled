using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeysSettingsManager : SettingsTab
{
    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;

    public override void SaveSettings()
    {

    }

    public override void RestoreDefaults()
    {

    }
}
