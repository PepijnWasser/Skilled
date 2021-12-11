using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHealth : MonoBehaviour
{
    public int stationHealth = 4;

    int startHealth = 0;

    public delegate void Damage(int health);
    public static event Damage updateStationHealth;

    public delegate void DamageLine();
    public static event DamageLine stationHalfWay;
    public static event DamageLine stationThreeQuarterWay;


    private void Awake()
    {
        ThreeWayLeverTask.taskDealDamage += TakeDamage;
        TwoWayLeverTask.taskDealDamage += TakeDamage;
        KeypadTask.taskDealDamage += TakeDamage;
        GameState.stationHealthUpdated += SetHealth;

        startHealth = stationHealth;
    }


    private void OnDestroy()
    {
        ThreeWayLeverTask.taskDealDamage -= TakeDamage;
        TwoWayLeverTask.taskDealDamage -= TakeDamage;
        KeypadTask.taskDealDamage -= TakeDamage;
        GameState.stationHealthUpdated -= SetHealth;
    }

    void TakeDamage(int amount)
    {
        Debug.Log("take damage");
        stationHealth -= amount;
        updateStationHealth?.Invoke(stationHealth);

        int halfHealth = startHealth / 2;
        int threeQuarterHealth = startHealth / 4 * 3;

        if(stationHealth == halfHealth)
        {
            stationHalfWay?.Invoke();
        }
        if (stationHealth == threeQuarterHealth)
        {
            stationThreeQuarterWay?.Invoke();
        }
    }

    void SetHealth(int health)
    {
        stationHealth = health;
    }
}
