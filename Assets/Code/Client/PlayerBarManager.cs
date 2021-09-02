using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarManager : MonoBehaviour
{

    public Image backgroundColor;
    public Text nameField;
    public Button nextColorButton;

    public void SelectNextColor()
    {
        GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>().UpdatePlayerColorRequest();
    }

    void SelectPreviousColor()
    {

    }

    public void SwitchColor(Color newColor)
    {
        backgroundColor.color = newColor;
    }

    public void SetName(string name)
    {
        nameField.text = name;
    }

    public void SetOwnership(bool isPlayer)
    {
        nextColorButton.gameObject.SetActive(isPlayer);
    }
}
