using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLobby : MonoBehaviour
{
    MySceneManager sceneManager;

    private void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
    }

    public void LoadLobbyScene()
    {
        sceneManager.LoadScene("LobbyScene");
    }
}
