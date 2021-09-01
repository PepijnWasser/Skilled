using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyState : MonoBehaviour
{
    private LocalHostClient clientnetwork;

    public Text playerCountUIElement;


    private void Awake()
    {
        clientnetwork = GameObject.FindObjectOfType<LocalHostClient>().GetComponent<LocalHostClient>();
        Debug.Log(clientnetwork);
    }

    private void Update()
    {        
        try
        {
            if (clientnetwork != null && clientnetwork.client.Connected && clientnetwork.client.Available > 0)
            {
                byte[] inBytes = StreamUtil.Read(clientnetwork.client.GetStream());
                Packet inPacket = new Packet(inBytes);

                var tempOBJ = inPacket.ReadObject();

                
                if (tempOBJ is UpdatePlayerCountMessage)
                {
                    UpdatePlayerCountMessage message = tempOBJ as UpdatePlayerCountMessage;
                    HandlePlayerCountMessage(message);
                }
            }
    
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (clientnetwork.client.Connected)
            {
                clientnetwork.client.Close();
            }

        }
        
    }

    void HandlePlayerCountMessage(UpdatePlayerCountMessage message)
    {
        playerCountUIElement.text = message.playerCount.ToString();
    }
}
