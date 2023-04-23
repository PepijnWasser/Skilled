using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindArraySetter : MonoBehaviour
{
    public List<KeybindSetter> keybindSetters;

    public void StartRebinding()
    {
        foreach(KeybindSetter setter in keybindSetters)
        {
            setter.StartRebinding();
        }
    }
}
