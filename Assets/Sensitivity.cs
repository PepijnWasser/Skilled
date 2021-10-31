using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivity : MonoBehaviour
{
    GameObject player2;
    public Slider HealthSlider;
    // Start is called before the first frame update
    void Start()
    {
        HealthSlider.minValue = 1;
        HealthSlider.maxValue = 400;
        GameManager.playerMade += setPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void sensitive (float value)
        {
        if (player2 == null)
        {
            Debug.Log("test");

        }
        else
        {
            player2.GetComponent<PlayerRotation>().sensitivity = value;
        }
    }
   void setPlayer(GameObject player, Camera cam)
    {
        player2 = player;
        
    }
}
