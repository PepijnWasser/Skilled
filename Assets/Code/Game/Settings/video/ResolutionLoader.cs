using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionLoader : VideoSetting
{
    public Resolution[] resolutions;

    protected override void Awake()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        dropdown.AddOptions(options);

        base.Awake();
    }

    protected override void SetVisualsToSavedValues()
    {
        int value1 = PlayerPrefs.GetInt("resolutionX");
        int value2 = PlayerPrefs.GetInt("resolutionY");


        if(resolutions != null)
        {
            int currentResolution = 0;

            for (int i = 0; i < resolutions.Length - 1; i++)
            {
                if (value1 == 0 && value2 == 0)
                {
                    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    {
                        currentResolution = i;
                    }
                }
                else
                {
                    if (resolutions[i].width == value1 && resolutions[i].height == value2)
                    {
                        currentResolution = i;
                    }
                }
            }
            dropdown.SetValueWithoutNotify(currentResolution);
        }
    }
}
