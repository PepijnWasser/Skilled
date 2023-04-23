using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverNamer : MonoBehaviour
{
    List<string> availableNames;

    public List<string> names = new List<string>()
    {
        "turbo",
        "lights",
        "beep boop sounds",
    };

    public void Initialize()
    {
        availableNames = new List<string>(names);
    }

    public string GetName()
    {
        if (availableNames.Count > 0)
        {
            string name = Extensions.RandomListItem(availableNames);
            availableNames.Remove(name);
            return name;
        }
        else
        {
            return "no name found";
        }
    }
}
