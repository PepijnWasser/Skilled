using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatFieldInputUpdater : InputfieldInputUpdater
{
    protected override void Update()
    {
        if (inputField.isFocused && isfocused == false)
        {
            originalActionMap = InputManager.activeAction;
            InputManager.ToggleActionMap(InputManager.chat);
            isfocused = true;
        }
        else if (!inputField.isFocused && isfocused == true)
        {
            InputManager.ToggleActionMap(originalActionMap);
            isfocused = false;
        }
    }
}
