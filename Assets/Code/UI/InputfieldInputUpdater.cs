using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputfieldInputUpdater : MonoBehaviour
{
    InputField inputField;

    public bool isfocused = false;

    InputActionMap originalActionMap;

    private void Start()
    {
        inputField = GetComponent<InputField>();
    }

    private void Update()
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
