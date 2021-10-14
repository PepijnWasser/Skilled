using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadCodeEnterer : MonoBehaviour
{
    public Text display;

    public string message = "";
    public string nameMessage;
    string errorMessage = "Error....";


    public void AddDigit(int digit)
    {
        message = message + digit;
        display.text = message;
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
        }
    }

    public void SetWelcomeMessage(string message)
    {
        nameMessage = message;
        DisplayWelcomeMessage();
    }
}
