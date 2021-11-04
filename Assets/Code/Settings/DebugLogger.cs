using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = Screen.width.ToString();
    }
}
