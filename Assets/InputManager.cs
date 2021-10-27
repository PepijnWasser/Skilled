using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    public static InputActionMap mainMenu;
    public static InputActionMap game;
    public static InputActionMap inputField;

    List<InputActionMap> actionMaps = new List<InputActionMap>();

    public static Controls controls;

    public static InputActionMap activeAction;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        controls = new Controls();

        mainMenu = controls.MainMenu;
        game = controls.Game;
        inputField = controls.InputField;

        actionMaps.Add(mainMenu);
        actionMaps.Add(game);
        actionMaps.Add(inputField);

        ToggleActionMap(mainMenu);
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (!actionMap.enabled)
        {
            controls.Disable();
            actionMap.Enable();
            SetActiveMap(actionMap);
        }
    }

    static void SetActiveMap(InputActionMap action)
    {
        activeAction = action;
    }
}
