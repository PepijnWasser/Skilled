using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPrefabManager : MonoBehaviour
{
    public Text nameField;
    public Text timeField;
    public Text messageField;

    public void Setmessage(string name, string time, string message)
    {
        nameField.text = name + ":";
        timeField.text = time;
        messageField.text = message;

        RectTransform rect = this.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, messageField.preferredHeight + 40);
    }
}
