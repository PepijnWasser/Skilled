using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListState : MonoBehaviour
{
    private LocalHostClient clientnetwork;

    public Text playerMessageUIElement;

    private void Awake()
    {
        clientnetwork = GameObject.FindObjectOfType<LocalHostClient>().GetComponent<LocalHostClient>();
    }
}
