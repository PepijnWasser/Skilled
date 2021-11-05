using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindSetter : MonoBehaviour
{
    public InputActionReference actionToChange;
    public Text nameText;

    public GameObject startRebindObjecet;
    public GameObject waitingForInputObject;

    InputActionMap originalActionMap;

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        InputManager.bindingSet += Setbinding;
    }

    private void OnDestroy()
    {
        InputManager.bindingSet -= Setbinding;
    }


    public void StartRebinding()
    {
        startRebindObjecet.SetActive(false);
        waitingForInputObject.SetActive(true);

        originalActionMap = InputManager.activeAction;
        InputManager.SetActiveActionMap(InputManager.rebind);
        rebindingOperation = actionToChange.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => Complete())
            .Start();
    }

    private void Complete()
    {
        rebindingOperation.Dispose();

        startRebindObjecet.SetActive(true);
        waitingForInputObject.SetActive(false);

        InputBinding newBinding = new InputBinding(actionToChange.action.bindings[0].effectivePath);
        Debug.Log(newBinding);
        InputManager.SetBinding(actionToChange.action.name, newBinding, this);
        InputManager.SetActiveActionMap(originalActionMap);
    }

    void Setbinding(string action, string newBinding, int index)
    {
        if(action == actionToChange.name)
        {
            Debug.Log(action);
            nameText.text = newBinding;
        }
    }

}
