using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettingsManager : SettingsTab
{
    public Dropdown overAllQualityDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown shadowQualityDropdown;
    public Dropdown resolutionDropdown;

    public ResolutionLoader resolutionLoader;

    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;

    public delegate void Reset();
    public static event Reset settingsReset;

    public void SetOverallQuality(int index)
    {
        SetOverallQuality(index, false);
    }

    public void SetOverallQuality(int index, bool startup)
    {
        SetShadowQuality(index, false, false);
        SetTextureQuality(index, false, false);

        if(overAllQualityDropdown.options.Count == 4 && index < 4)
        {
            overAllQualityDropdown.options.RemoveAt(overAllQualityDropdown.options.Count - 1);
        }
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public void SetShadowQuality(int index)
    {
        SetShadowQuality(index, true, false);
    }
    
    public void SetShadowQuality(int index, bool calledFromScript, bool startup)
    {
        int currentLevel = QualitySettings.GetQualityLevel();
        QualitySettings.SetQualityLevel(index);

        ShadowmaskMode shadowMaskMode = QualitySettings.shadowmaskMode;
        ShadowQuality shadowQuality = QualitySettings.shadows;
        ShadowResolution shadowResolution = QualitySettings.shadowResolution;
        float ShadowDistance = QualitySettings.shadowDistance;
        int shadowCascades = QualitySettings.shadowCascades;

        QualitySettings.SetQualityLevel(currentLevel);
        QualitySettings.shadowmaskMode = shadowMaskMode;
        QualitySettings.shadows = shadowQuality;
        QualitySettings.shadowResolution = shadowResolution;
        QualitySettings.shadowDistance = ShadowDistance;
        QualitySettings.shadowCascades = shadowCascades;

        if (calledFromScript == true)
        {
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = "custom";
            optionDatas.Add(optionData);

            overAllQualityDropdown.AddOptions(optionDatas);
            overAllQualityDropdown.SetValueWithoutNotify(overAllQualityDropdown.options.Count - 1);
        }

        shadowQualityDropdown.SetValueWithoutNotify(index);
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public void SetTextureQuality(int index)
    {
        SetTextureQuality(index, true, false);
    }

    public void SetTextureQuality(int index, bool calledFromScript, bool startup)
    {
        int currentLevel = QualitySettings.GetQualityLevel();
        QualitySettings.SetQualityLevel(index);

        int pixelLightCOunt = QualitySettings.pixelLightCount;
        AnisotropicFiltering aniStrophicFiltering = QualitySettings.anisotropicFiltering;
        bool realTimeReflectionProbes = QualitySettings.realtimeReflectionProbes;
        bool billBoardsFaceCameraPosition = QualitySettings.billboardsFaceCameraPosition;

        QualitySettings.SetQualityLevel(currentLevel);
        QualitySettings.pixelLightCount = pixelLightCOunt;
        QualitySettings.anisotropicFiltering = aniStrophicFiltering;
        QualitySettings.realtimeReflectionProbes = realTimeReflectionProbes;
        QualitySettings.billboardsFaceCameraPosition = billBoardsFaceCameraPosition;

        if (calledFromScript == true)
        {
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = "custom";
            optionDatas.Add(optionData);

            overAllQualityDropdown.AddOptions(optionDatas);
            overAllQualityDropdown.SetValueWithoutNotify(overAllQualityDropdown.options.Count - 1);
        }

        textureQualityDropdown.SetValueWithoutNotify(index);
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public void SetAntialiasing(int value)
    {
        SetAntialiasing(value, false);
    }

    public void SetAntialiasing(int value, bool startup)
    {
        if (value == 0)
        {
            QualitySettings.antiAliasing = 0;
        }
        else if (value == 1)
        {
            QualitySettings.antiAliasing = 2;
        }
        else if (value == 2)
        {
            QualitySettings.antiAliasing = 4;
        }
        else if (value == 3)
        {
            QualitySettings.antiAliasing = 8;
        }
        if (!startup)
        {
            changeCreated?.Invoke(this);    
        }
    }

    public void SetVsync(int value)
    {
        SetVsync(value, false);
    }

    public void SetVsync(int value, bool startup)
    {
        if(value == 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public void SetResolution(int value)
    {
        SetResolution(value, false);
    }

    public void SetResolution(int value, bool startup)
    {
        Resolution resolution = resolutionLoader.resolutions[value];
        FullScreenMode mode = Screen.fullScreenMode;
        bool fullScreen = Screen.fullScreen;

        Screen.SetResolution(resolution.width, resolution.height, false);
        Screen.fullScreenMode = mode;
        Screen.fullScreen = fullScreen;
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    public void SetMode(int value)
    {
        SetMode(value, false);
    }

    public void SetMode(int value, bool startup)
    {
        if(value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
        else if(value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreen = true;
        }
        else if(value == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        if (!startup)
        {
            changeCreated?.Invoke(this);
        }
    }

    private void Update()
    {
        Debug.Log(Screen.currentResolution);
        Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()] + " Cascades: " + QualitySettings.shadowCascades + " Pixel light count: " + QualitySettings.pixelLightCount + " V-Sync: " + QualitySettings.vSyncCount + " Antiailisaing: " + QualitySettings.antiAliasing);
    }

    public override void SaveSettings()
    {
        if(Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            PlayerPrefs.SetInt("fullScreenMode", 0);
        }
        else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            PlayerPrefs.SetInt("fullScreenMode", 1);
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            PlayerPrefs.SetInt("fullScreenMode", 2);
        }
        PlayerPrefs.SetInt("resolutionX", Screen.width);
        PlayerPrefs.SetInt("resolutionY", Screen.height);
        PlayerPrefs.SetInt("VSync", QualitySettings.vSyncCount);
        int antiAliasing = QualitySettings.antiAliasing;
        if(antiAliasing == 0)
        {
            PlayerPrefs.SetInt("antiAliasing", 0);
        }
        else if(antiAliasing == 2)
        {
            PlayerPrefs.SetInt("antiAliasing", 1);
        }
        else if(antiAliasing == 4)
        {
            PlayerPrefs.SetInt("antiAliasing", 2);
        } 
        else if(antiAliasing == 8)
        {
            PlayerPrefs.SetInt("antiAliasing", 3);
        }
        PlayerPrefs.SetInt("overallQuality", overAllQualityDropdown.value);
        PlayerPrefs.SetInt("shadowQuality", shadowQualityDropdown.value);
        PlayerPrefs.SetInt("textureQuality", textureQualityDropdown.value);
    }

    public override void RestoreDefaults()
    {
        SetOverallQuality(2);
        SetAntialiasing(0);
        SetVsync(1);
        SetResolution(resolutionLoader.resolutions.Length - 1);
        SetMode(2);
        settingsReset?.Invoke();
        changeCreated?.Invoke(this);
    }
}