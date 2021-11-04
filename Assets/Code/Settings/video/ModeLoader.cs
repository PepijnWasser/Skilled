using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeLoader : DropDownLoader
{
    protected override void Awake()
    {
        dropdownName = "fullScreenMode";
        base.Awake();
    }

    protected override void HandleValue()
    {
        base.HandleValue();
        manager.SetMode(value, true);
    }

    protected override void Reset()
    {
        StartCoroutine("SetValue");
    }

    IEnumerator SetValue()
    {
        yield return new WaitForFixedUpdate();

        base.Reset();
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            dropdown.SetValueWithoutNotify(0);
        }
        else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            dropdown.SetValueWithoutNotify(1);
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            dropdown.SetValueWithoutNotify(2);
        }
        else
        {
            dropdown.SetValueWithoutNotify(2);
        }
        yield return null;
    }
}
