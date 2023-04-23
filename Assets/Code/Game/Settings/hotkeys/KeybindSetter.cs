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

            .WithCancelingThrough("<Mouse>/LeftButton")

            .OnMatchWaitForAnother(0.1f)

            .OnComplete(operation => Complete())
            .OnCancel(operation => Cancel())

            .Start();
    }


    private void Cancel()
    {
        rebindingOperation.Dispose();
        startRebindObjecet.SetActive(true);
        waitingForInputObject.SetActive(false);
    }

    private void Complete()
    {
        rebindingOperation.Dispose();

        startRebindObjecet.SetActive(true);
        waitingForInputObject.SetActive(false);

        InputBinding newBinding = new InputBinding(actionToChange.action.bindings[0].effectivePath);
        InputManager.SetBinding(actionToChange.action.name, newBinding);
        InputManager.SetActiveActionMap(originalActionMap);
    }

    void Setbinding(string action, string newBinding, int index)
    {
        if (action == actionToChange.name)
        {
            nameText.text = newBinding;
        }
    }
}
