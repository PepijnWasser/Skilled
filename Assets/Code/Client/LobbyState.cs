using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class LobbyState : MonoBehaviour
{
    private LocalHostClient clientnetwork;

    public Text playerCountUIElement;
    public Text serverNameUIElement;
    public Image playerColorUIElement;

    private void Awake()
    {
        clientnetwork = GameObject.FindObjectOfType<LocalHostClient>().GetComponent<LocalHostClient>();
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
                else if(tempOBJ is UpdateServerNameMessage)
                {
                    UpdateServerNameMessage message = tempOBJ as UpdateServerNameMessage;
                    HandleServerNameMessage(message);
                }
                else if(tempOBJ is UpdateColorMessage)
                {
                    UpdateColorMessage message = tempOBJ as UpdateColorMessage;
                    HandleUpdateColorMessage(message);
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

    void HandleServerNameMessage(UpdateServerNameMessage message)
    {
        serverNameUIElement.text = message.serverName + "'s server";
    }

    void HandleUpdateColorMessage(UpdateColorMessage message)
    {
        Debug.Log(message.color);
        System.Drawing.Color c = System.Drawing.Color.FromName(message.color);
        playerColorUIElement.color = new Color32(c.R, c.G, c.B, c.A);
    }

    public void UpdatePlayerColorRequest()
    {
        UpdateColorMessage message = new UpdateColorMessage();
        clientnetwork.SendObject(message);
    }
}
