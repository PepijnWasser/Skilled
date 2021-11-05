using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputActionMap mainMenu;
    public static InputActionMap game;
    public static InputActionMap inputField;
    public static InputActionMap focusable;
    public static InputActionMap chat;
    public static InputActionMap rebind;
    public static InputActionMap settings;

    public static List<InputActionMap> actionMapsToChange = new List<InputActionMap>();
    public static List<InputActionMap> savedActionMaps = new List<InputActionMap>();

    public static Controls controlsToChange;
    public static Controls savedControls;

    public static InputActionMap activeAction;

    public delegate void Changes();
    public static event Changes changeCreated;

    public delegate void BindingsReset(string action, string newBinding, int index = 0);
    public static event BindingsReset bindingSet;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Starter();
    }

    public static void Starter()
    {
        controlsToChange = new Controls();
        savedControls = new Controls();

        actionMapsToChange = GetMapsFromControls(controlsToChange);
        savedActionMaps = GetMapsFromControls(savedControls, true);

        GetSavedValues(actionMapsToChange);
        GetSavedValues(savedActionMaps);

        SetActiveActionMap(mainMenu);

        SetSavedValues();
    }

    public static void ResetChanges()
    {
        controlsToChange = new Controls();
        actionMapsToChange = GetMapsFromControls(controlsToChange);
        GetSavedValues(actionMapsToChange);
        SetSavedValues();
    }

    //sets the actions in the actionmap to the saved hotkey
    public static void GetSavedValues(List<InputActionMap> actionMaps)
    {
        foreach (InputActionMap map in actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string readString = PlayerPrefs.GetString(map.name + "/" + action.name + i);
                        if(readString != "")
                        {
                            InputBinding newBinding = new InputBinding(readString);
                            SetInitialCompositeValue(action, i, newBinding);
                        }
                        else
                        {
                            InputBinding newBinding = new InputBinding(action.bindings[i].path);
                            SetInitialCompositeValue(action, i, newBinding);
                        }
                    }
                }
                else
                {
                    string readString = PlayerPrefs.GetString(map.name + "/" + action.name);
                    if(readString != "")
                    {
                        InputBinding newBinding = new InputBinding(readString);
                        SetInitialValue(action, newBinding);
                    }
                    else
                    {
                        InputBinding newBinding = new InputBinding(action.bindings[0].path);
                        SetInitialValue(action, newBinding);
                    }

                }
            }
        }
    }

    //sets the setter text to the hotkey from the actionMap
    public static void SetSavedValues()
    {
        foreach (InputActionMap map in savedActionMaps)
        {
            foreach (InputAction action in map)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string binding = InputControlPath.ToHumanReadableString(action.bindings[i].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                        bindingSet?.Invoke(map.name + "/" + action.name, binding, i);
                    }
                }
                else
                {
                    string binding = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    bindingSet?.Invoke(map.name + "/" + action.name, binding);
                }
            }
        }
    }

    public static void SetInitialValue(InputAction actionToChange, InputBinding newKeyBinding)
    {
        actionToChange.ApplyBindingOverride(newKeyBinding.path);
    }

    static void SetInitialCompositeValue(InputAction action, int actionIndex, InputBinding newKeyBinding)
    {
        if (action.bindings.Count > actionIndex)
        {
            action.ApplyBindingOverride(actionIndex, newKeyBinding.path);
        }
        else
        {
            Debug.Log("index not found");
        }
    }

    public static void SetActiveActionMap(InputActionMap actionMap)
    {
        if (!actionMap.enabled)
        {
            savedControls.Disable();
            actionMap.Enable();
            activeAction = actionMap;
        }
    }

    //set bingdings of controlsToChange
    public static void SetBinding(string actionName, InputBinding newKeyBinding, KeybindSetter keybindSetter)
    {
        List<InputAction> actionsToChange = new List<InputAction>();

        foreach (InputActionMap map in actionMapsToChange)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.name == actionName)
                {
                    actionsToChange.Add(action);
                }
            }
        }

        foreach(InputAction actionToChange in actionsToChange)
        {
            actionToChange.ApplyBindingOverride(newKeyBinding.path);

            string binding = InputControlPath.ToHumanReadableString(actionToChange.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            bindingSet?.Invoke(actionToChange.actionMap.name + "/" + actionToChange.name, binding);
        }
        
        changeCreated?.Invoke();

    }

    public static void SetBindingComposite(string actionName, int actionIndex, InputBinding newKeyBinding, KeybindSetterComposite keybindSetter)
    {
        InputAction actionToChange = null;

        foreach (InputActionMap map in actionMapsToChange)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.name == actionName)
                {
                    actionToChange = action;
                }
            }
        }

        if (actionToChange.bindings.Count > actionIndex)
        {
            actionToChange.ApplyBindingOverride(actionIndex, newKeyBinding.path);

            keybindSetter.nameText.text = InputControlPath.ToHumanReadableString(actionToChange.bindings[actionIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

            string binding = InputControlPath.ToHumanReadableString(actionToChange.bindings[actionIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            bindingSet?.Invoke(actionToChange.actionMap.name + "/" + actionToChange.name, binding, actionIndex);



            changeCreated?.Invoke();
        }
        else
        {
            Debug.Log("index not found");
        }
    }


    //sets the values of a given list according to playerprefs
    static void GetBindingsFromPlayerPrefs(List<InputActionMap> actionMaps)
    {
        foreach (InputActionMap map in actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string readString = PlayerPrefs.GetString(map.name + "/" + action.name + i);
                        InputBinding newBinding = new InputBinding(readString);

                        SetInitialCompositeValue(action, i, newBinding);
                    }
                }
                else
                {
                    string readString = PlayerPrefs.GetString(map.name + "/" + action.name);
                    InputBinding newBinding = new InputBinding(readString);

                    SetInitialValue(action, newBinding);
                }
            }
        }
    }

    static List<InputActionMap> GetMapsFromControls(Controls controls, bool setPublicField = false)
    {
        List<InputActionMap> maps = new List<InputActionMap>();

        if (setPublicField)
        {
            mainMenu = controls.MainMenu;
            game = controls.Game;
            inputField = controls.InputField;
            focusable = controls.Focusable;
            chat = controls.Chat;
            rebind = controls.Rebind;
            settings = controls.Settings;

            maps.Add(mainMenu);
            maps.Add(game);
            maps.Add(inputField);
            maps.Add(focusable);
            maps.Add(chat);
            maps.Add(rebind);
            maps.Add(settings);
        }
        else
        {
            maps.Add(controls.MainMenu);
            maps.Add(controls.Game);
            maps.Add(controls.InputField);
            maps.Add(controls.Focusable);
            maps.Add(controls.Chat);
            maps.Add(controls.Rebind);
            maps.Add(controls.Settings);
        }
        return maps;
    }


    //reset the controlsToChange to the original keybindings
    public static void RestoreToDefault()
    {
        controlsToChange = new Controls();

        actionMapsToChange = GetMapsFromControls(controlsToChange, false);

        //send all the original actions to the setters
        foreach (InputActionMap map in actionMapsToChange)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string binding = InputControlPath.ToHumanReadableString(action.bindings[i].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                        bindingSet?.Invoke(map.name + "/" + action.name, binding, i);
                    }
                }
                else
                {
                    string binding = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    bindingSet?.Invoke(map.name + "/" + action.name, binding);
                }
            }
        }

        changeCreated?.Invoke();
    }

    public static void SaveBindings()
    {
        foreach (InputActionMap map in actionMapsToChange)
        {
            foreach (InputAction action in map.actions)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        PlayerPrefs.SetString(map.name + "/" + action.name + i, action.bindings[i].effectivePath);
                    }
                }
                else
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        PlayerPrefs.SetString(map.name + "/" + action.name, action.bindings[i].effectivePath);
                    }
                }
            }
        }

        GetBindingsFromPlayerPrefs(savedActionMaps);
    }
}
