using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject background;
    public TypeSelectorManager typeSelectionManager;

    public delegate void OnDisable();
    public static event OnDisable disabled;

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

        background.SetActive(true);
    }

    public void Close()
    {
        inSettings = false;
        settingsMenuGameObject.enabled = false;

        background.SetActive(false);
        disabled?.Invoke();
    }
}
