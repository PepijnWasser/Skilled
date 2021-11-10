using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    MySceneManager sceneManager;

    private void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
    }

    public void LoadMainMenu()
    {
        sceneManager.LoadScene("Main Menu");
    }
}
