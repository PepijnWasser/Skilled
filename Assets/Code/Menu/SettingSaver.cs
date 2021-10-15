using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSaver : MonoBehaviour
{
    public List<SettingTab> tabsWithChanges;
    public Button saveButton;

    private void Start()
    {
        SoundSettingsManager.changeCreated += SetButtonStatus;
    }

    private void OnDestroy()
    {
        SoundSettingsManager.changeCreated -= SetButtonStatus;
    }

    void SetButtonStatus(SettingTab tab)
    {
        if(tabsWithChanges.Contains(tab) == false)
        {
            tabsWithChanges.Add(tab);
        }
        saveButton.enabled = true;
    }

    public void saveSettings()
    {
        foreach(SettingTab tab in tabsWithChanges)
        {

                tab.SaveSettings();
            
        }

        saveButton.enabled = false;
    }
}
