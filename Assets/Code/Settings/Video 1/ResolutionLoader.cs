using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionLoader : DropDownLoader
{
    public Resolution[] resolutions;
    public Text text;

    protected override void Awake()
    {
        base.Awake();

        dropdownName = "resolution";

        int valX = PlayerPrefs.GetInt(dropdownName + "X");
        int valY = PlayerPrefs.GetInt(dropdownName + "Y");
    
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(valX == 0 && valY == 0)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            else
            {
                if (resolutions[i].width == valX && resolutions[i].height == valY)
                {
                    currentResolutionIndex = i;
                }
            }
        }

        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(currentResolutionIndex);

        manager.SetResolution(currentResolutionIndex, true);
    }

    protected override void Reset()
    {
        base.Reset();
        StartCoroutine("SetValue");
    }

    IEnumerator SetValue()
    {
        yield return new WaitForFixedUpdate();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        dropdown.SetValueWithoutNotify(currentResolutionIndex);

        yield return null;
    }
}
