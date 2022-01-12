using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipHealthSlider : MonoBehaviour
{
    public Text HealthText;
    public Slider HealthSlider;

    StationHealth stationHealth;

    private void Awake()
    {
        stationHealth = GameObject.FindObjectOfType<StationHealth>();
    }

    void Start()
    {
        HealthSlider.maxValue = stationHealth.GetStationHealth();

        StationHealth.stationTookDamage += UpdateSlider;
        GameState.stationHealthUpdated += UpdateSlider;

        UpdateSlider(GameObject.FindObjectOfType<StationHealth>().GetStationHealth());
    }

    private void OnDestroy()
    {
        StationHealth.stationTookDamage -= UpdateSlider;
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
