using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndView : MonoBehaviour
{
    public Text tasksCompletedText;

    public void SetTasksCompleted(int amount)
    {
        tasksCompletedText.text = amount.ToString();
    }
}
