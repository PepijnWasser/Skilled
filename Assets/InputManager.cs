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

    public static List<InputActionMap> actionMapsToChange = new List<InputActionMap>();
    public static List<InputActionMap> savedActionMaps = new List<InputActionMap>();

    public static Controls controlsToChange;
    public static Controls savedControls;

    public static InputActionMap activeAction;

    public delegate void Changes();
    public static event Changes changeCreated;

    public delegate void BindingsReset(string action, string newBinding, int index = 0);
    public static event BindingsReset bindingsRestored;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        controlsToChange = new Controls();
        savedControls = new Controls();

        actionMapsToChange = GetMapsFromControls(controlsToChange);
        savedActionMaps = GetMapsFromControls(savedControls, true);

        GetSavedValues(actionMapsToChange);
        GetSavedValues(savedActionMaps);

        ToggleActionMap(mainMenu);
    }

    private void Start()
    {
        foreach(InputActionMap map in savedActionMaps)
        {
            foreach(InputAction action in map)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string binding = InputControlPath.ToHumanReadableString(action.bindings[i].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                        bindingsRestored?.Invoke(map.name + "/" + action.name, binding, i);
                    }
                }
                else
                {
                    string binding = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    bindingsRestored?.Invoke(map.name + "/" + action.name, binding);
                }
            }
        }
    }

    private void Update()
    {
        Debug.Log(savedControls.Game.Movement.bindings[1] + " saved");
        Debug.Log(controlsToChange.Game.Movement.bindings[1] + " change");
    }

    static void GetSavedValues(List<InputActionMap> actionMaps)
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

    public static void SetInitialValue(InputAction actionToChange, InputBinding newKeyBinding)
    {
        actionToChange.ApplyBindingOverride(newKeyBinding.path);
    }

    static void SetInitialCompositeValue(InputAction action, int actionIndex, InputBinding newKeyBinding)
    {
        if (action.bindings.Count > actionIndex)
        {
            action.ApplyBindingOverride(actionIndex, newKeyBinding.path);
          //  Debug.Log(action.bindings[actionIndex].effectivePath);
        }
        else
        {
            Debug.Log("index not found");
        }
    }

    public static void ToggleActionMap(InputActionMap actionMap)
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

        actionToChange.ApplyBindingOverride(newKeyBinding.path);

        string binding = InputControlPath.ToHumanReadableString(actionToChange.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        bindingsRestored?.Invoke(actionToChange.actionMap.name + "/" + actionToChange.name, binding);
        
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
            bindingsRestored?.Invoke(actionToChange.actionMap.name + "/" + actionToChange.name, binding, actionIndex);



            changeCreated?.Invoke();
        }
        else
        {
            Debug.Log("index not found");
        }
    }

    //reset the controlsToChange to the original keybindings
    public static void RestoreToDefault()
    {
        controlsToChange = new Controls();

        actionMapsToChange = GetMapsFromControls(controlsToChange, false);

        changeCreated?.Invoke();

        //send all the original actions to the setters
        foreach(InputActionMap map in actionMapsToChange)
        {
            foreach(InputAction action in map.actions)
            {
                if (action.bindings[0].isComposite)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        string binding = InputControlPath.ToHumanReadableString(action.bindings[i].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                        bindingsRestored?.Invoke(map.name + "/" + action.name, binding, i);
                    }
                }
                else
                {
                    string binding = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    bindingsRestored?.Invoke(map.name + "/" + action.name, binding);
                }
            }
        }
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

            maps.Add(mainMenu);
            maps.Add(game);
            maps.Add(inputField);
            maps.Add(focusable);
            maps.Add(chat);
            maps.Add(rebind);
        }
        else
        {
            maps.Add(controls.MainMenu);
            maps.Add(controls.Game);
            maps.Add(controls.InputField);
            maps.Add(controls.Focusable);
            maps.Add(controls.Chat);
            maps.Add(controls.Rebind);
        }
        return maps;
    }
}
