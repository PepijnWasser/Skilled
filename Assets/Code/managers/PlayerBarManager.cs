using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarManager : MonoBehaviour
{

    public Text backgroundColor;
    public Text nameField;
    public Button nextColorButton;
    public Button previousColorButton;
    public Button muteButton;

    public Sprite mutedIcon;
    public Sprite unMutedIcon;

    public void SelectNextColor()
    {
        GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>().SendUpdateColorRequest(1);
    }

    public void SelectPreviousColor()
    {
        GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>().SendUpdateColorRequest(-1);
    }

    public void SwitchMute()
    {
        GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>().SendUpdateMuteRequest();
    }

    public void SwitchColor(Color newColor)
    {
        backgroundColor.color = newColor;
    }

    public void SetName(string name)
    {
        nameField.text = name;
    }

    public void SetMute(bool muted)
    {
        if (muted)
        {
            muteButton.gameObject.GetComponent<Image>().sprite = mutedIcon;
        }
        else
        {
            muteButton.gameObject.GetComponent<Image>().sprite = unMutedIcon;
        }
    }

    public void SetOwnership(bool isPlayer)
    {
        nextColorButton.gameObject.SetActive(isPlayer);
        previousColorButton.gameObject.SetActive(isPlayer);
        muteButton.interactable = isPlayer;
    }
}
