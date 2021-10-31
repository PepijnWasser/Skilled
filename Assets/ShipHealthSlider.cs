using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipHealthSlider : MonoBehaviour
{
    GameObject ShipStatus;
    int ShipHealth;
    public Text HealthText;
    public Slider HealthSlider;
    //public GameObject EndScreenActive;

    void Start()
    {
        HealthSlider.maxValue = 100;
        //EndScreenActive.SetActive(false);
    }

    void Update()
    {
        ShipStatus = GameObject.Find("stationHealth");
        StationHealth stationHealth = ShipStatus.GetComponent<StationHealth>();
        ShipHealth = stationHealth.stationHealth;
        if (ShipHealth >= 0)
            {
                HealthText.text = ShipHealth.ToString();
                HealthSlider.value = ShipHealth;
            }
            if (ShipHealth <= 0)
            {
            Debug.Log("Game Over");
            //EndScreenActive.SetActive(true);
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main Menu");
    }
    }
