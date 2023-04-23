using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadCodeEnterer : MonoBehaviour
{
    public Text display;
    public int id;

    public string message = "";
    public string nameMessage;
    string errorMessage = "Error....";

    public AudioSource enterAudio;

    public delegate void MessageUpdated(int id, string message);
    public static event MessageUpdated messageUpdated;

    private void Awake()
    {
        GameState.updateKeypadScreen += SetMessage;
    }

    private void OnDestroy()
    {
        GameState.updateKeypadScreen -= SetMessage;
    }

    public void AddDigit(int digit)
    {
        message = message + digit;
        display.text = message;

        messageUpdated?.Invoke(id, message);

        enterAudio.Play();
    }

    public void DisplayWelcomeMessage()
    {
        display.text = nameMessage;
        message = "";
    }

    public void DisplayErrorMessage()
    {
        display.text = errorMessage;
        message = "";
    }

    public void RemoveDigit()
    {
        if(message.Length > 0)
        {
            message = message.Remove(message.Length - 1);
            display.text = message;

            messageUpdated?.Invoke(id, message);

            enterAudio.Play();
        }
    }

    public void SetWelcomeMessage(string message)
    {
        nameMessage = message;
        DisplayWelcomeMessage();
    }

    void SetMessage(int _id, string _message)
    {
        if(id == _id)
        {
            int oldLength = message.Length;
            message = _message;
            display.text = message;

            if(oldLength != message.Length)
            {
                enterAudio.Play();
            }
        }
    }
}
