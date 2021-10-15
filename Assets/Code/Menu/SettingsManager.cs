using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    Canvas settingsMenuGameObject;

    bool inSettings = false;

    private void Start()
    {
        settingsMenuGameObject = GetComponent<Canvas>();
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSettings)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    void Open()
    {
        inSettings = true;
        settingsMenuGameObject.enabled = true;
    }

    void Close()
    {
        inSettings = false;
        settingsMenuGameObject.enabled = false;
    }
}
