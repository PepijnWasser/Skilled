using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeybindSetterComposite : MonoBehaviour
{
    public InputActionReference actionToChange;
    public Text nameText;

    public GameObject startRebindObjecet;
    public GameObject waitingForInputObject;

    public int index;

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
        rebindingOperation = actionToChange.action.PerformInteractiveRebinding(index)
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


        InputBinding newBinding = new InputBinding(actionToChange.action.bindings[index].effectivePath);
        InputManager.SetBindingComposite(actionToChange.action.name, index, newBinding, this);
        InputManager.SetActiveActionMap(originalActionMap);
    }

    void Setbinding(string action, string newBinding, int actionIndex)
    {
        if (action == actionToChange.name && actionIndex == index)
        {
            //Debug.Log(nameText + " " + newBinding +  this.gameObject.name);
            nameText.text = newBinding;
        }
    }
}
