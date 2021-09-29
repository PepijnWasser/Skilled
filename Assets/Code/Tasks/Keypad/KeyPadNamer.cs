using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadNamer : MonoBehaviour
{
    List<string> availableNames;
    
    public List<string> names = new List<string>()
    {
        "adminAccount",
        "Space coordinates",
        "communication array channel",
    };

    private void Start()
    {
        availableNames = new List<string>(names);
    }

    public string GetName()
    {
        if(availableNames.Count > 0)
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
