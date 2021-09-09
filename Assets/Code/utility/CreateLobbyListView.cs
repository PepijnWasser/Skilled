using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLobbyListView : MonoBehaviour
{
    public GameObject lobbyListViewPrefab;

    public void CreateLobbyListViewItem()
    {
        Instantiate(lobbyListViewPrefab);
        Destroy(this.gameObject);
    }
}
