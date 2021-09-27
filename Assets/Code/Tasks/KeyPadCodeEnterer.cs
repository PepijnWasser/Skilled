using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPadCodeEnterer : MonoBehaviour
{
    public KeypadTask task;
    public Text display;

    public string message = "";

    public void AddDigit(int digit)
    {
        message = message + digit;
        display.text = message;
    }

    public void DisplayWelcomeMessage()
    {
        display.text = "Welcome";
        message = "";
    }

    public void DisplayErrorMessage()
    {
        display.text = "Error...";
        message = "";
    }
}
