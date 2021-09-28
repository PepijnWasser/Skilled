using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    bool showCursor;
    public Image cursor;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (showCursor)
        {
            cursor.enabled = true;
            showCursor = false;
        }
        else
        {
            cursor.enabled = false;
        }
    }

    public void ShowCursor()
    {
        showCursor = true;
    }
}
