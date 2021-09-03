using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPlayerName : MonoBehaviour
{
    public Text nameField;
    public GameObject CJScreenPrefab;

    public bool CheckValidName()
    {
        if(nameField.text != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ValidatePlayerName()
    {
        if (CheckValidName())
        {
            Instantiate(CJScreenPrefab);
            GameObject.FindObjectOfType<LocalHostClient>().playerName = nameField.text;
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("invalid name");
        }
    }
}
