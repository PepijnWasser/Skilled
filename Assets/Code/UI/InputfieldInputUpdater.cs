using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputfieldInputUpdater : MonoBehaviour
{
    protected InputField inputField;

    public bool isfocused = false;

    protected InputActionMap originalActionMap;

    private void Start()
    {
        inputField = GetComponent<InputField>();
    }

    protected virtual void Update()
    {
        if (inputField.isFocused && isfocused == false)
        {
            originalActionMap = InputManager.activeAction;
            InputManager.ToggleActionMap(InputManager.inputField);
            isfocused = true;
        }
        else if(!inputField.isFocused && isfocused == true)
        {
            InputManager.ToggleActionMap(originalActionMap);
            isfocused = false;
        }
    }
}
