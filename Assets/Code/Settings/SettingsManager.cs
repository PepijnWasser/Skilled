using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
        InputManager.savedControls.MainMenu.OpenSettings.performed += _ => SwitchOpen();
        InputManager.savedControls.Game.OpenSettings.performed += _ => SwitchOpen();
        InputManager.savedControls.Focusable.OpenSettings.performed += _ => SwitchOpen();
        settingsMenuGameObject = GetComponent<Canvas>();

        DontDestroyOnLoad(this.gameObject);
        Close();
    }

    private void OnDestroy()
    {
        InputManager.savedControls.MainMenu.OpenSettings.performed -= _ => SwitchOpen();
        InputManager.savedControls.Game.OpenSettings.performed -= _ => SwitchOpen();
        InputManager.savedControls.Focusable.OpenSettings.performed -= _ => SwitchOpen();
    }

    public void SwitchOpen()
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
