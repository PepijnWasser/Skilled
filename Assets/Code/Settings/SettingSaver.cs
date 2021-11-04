using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSaver : MonoBehaviour
{
    public List<SettingsTab> tabsWithChanges;
    public Button saveButton;

    private void Start()
    {
        SoundSettingsManager.changeCreated += AddChanges;
        HotkeysSettingsManager.changeCreated += AddChanges;
        VideoSettingsManager.changeCreated += AddChanges;

        SettingsManager.disabled += ClearChanges;
    }


    private void OnDestroy()
    {
        SoundSettingsManager.changeCreated -= AddChanges;
        HotkeysSettingsManager.changeCreated -= AddChanges;
        VideoSettingsManager.changeCreated -= AddChanges;

        SettingsManager.disabled -= ClearChanges;
    }


    void AddChanges(SettingsTab tab)
    {
        if (tabsWithChanges.Contains(tab) == false)
        {
            tabsWithChanges.Add(tab);
        }
        TestButtonStatus();
    }

    void ClearChanges()
    {
        tabsWithChanges.Clear();
        TestButtonStatus();
    }

    void TestButtonStatus()
    {
        if(tabsWithChanges.Count > 0)
        {
            saveButton.enabled = true;
        }
        else
        {
            saveButton.enabled = false;
        }

    }

    public void saveSettings()
    {
        foreach(SettingsTab tab in tabsWithChanges)
        {
            tab.SaveSettings();    
        }
        tabsWithChanges.Clear();
        saveButton.enabled = false;
    }
}
