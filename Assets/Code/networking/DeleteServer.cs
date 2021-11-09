using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteServer : MonoBehaviour
{
    public void Leave()
    {
        if (ServerConnectionData.isOwner)
        {
            Destroy(GameObject.FindObjectOfType<LocalHostServer>().gameObject);
        }
    }
}
