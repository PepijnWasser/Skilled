using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipHealthSlider : MonoBehaviour
{
    public Text HealthText;
    public Slider HealthSlider;

    public StationHealth stationHealth;

    private void Awake()
    {
        stationHealth = GameObject.FindObjectOfType<StationHealth>();
        UpdateSlider(GameObject.FindObjectOfType<StationHealth>().stationHealth);
    }

    void Start()
    {
        HealthSlider.maxValue = stationHealth.stationHealth;

        StationHealth.updateStationHealth += UpdateSlider;
        GameState.stationHealthUpdated += UpdateSlider;
    }

    private void OnDestroy()
    {
        StationHealth.updateStationHealth -= UpdateSlider;
        GameState.stationHealthUpdated -= UpdateSlider;
    }


    void UpdateSlider(int newHealth)
    {
        if (newHealth > 0)
        {
            HealthText.text = newHealth.ToString();
            HealthSlider.value = newHealth;
        }
    }
}
