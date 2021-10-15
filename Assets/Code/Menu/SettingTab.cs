using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingTab : MonoBehaviour
{
    public bool hasChanges;

    public abstract void SaveSettings();
}
