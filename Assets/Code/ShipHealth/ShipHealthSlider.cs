using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipHealthSlider : MonoBehaviour
{
    public Text HealthText;
    public Slider HealthSlider;

    void Start()
    {
        HealthSlider.maxValue = StationHealth.stationHealth;

        StationHealth.updateStationHealth += UpdateSlider;
    }

    private void OnDestroy()
    {
        StationHealth.updateStationHealth -= UpdateSlider;
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
