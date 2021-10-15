using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject typePanel;
    public TypeSelectorManager typeSelectionManager;

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

        typeSelectionManager.OnOpen();
    }

    public void Close()
    {
        inSettings = false;
        settingsMenuGameObject.enabled = false;

        foreach (Transform child in typePanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
