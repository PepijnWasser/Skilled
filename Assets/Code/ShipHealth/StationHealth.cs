using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHealth : MonoBehaviour
{
    [SerializeField]
    private int startHealth = 0;

    public delegate void Damage(int newHealth);
    public static event Damage stationTookDamage;

    public delegate void DamageLine();
    public static event DamageLine stationHalfWay;
    public static event DamageLine stationThreeQuarterWay;

    private int halfHealth;
    private int quarterHealth;

    private int stationHealth;

    //we subsribe to the event launched when a task deals damage, and the event when we get a stationHealthUpdate from the server
    //we also calculate the halfHealth and quaterHealth
    private void Awake()
    {
        ThreeWayLeverTask.taskDealDamage += TakeDamage;
        TwoWayLeverTask.taskDealDamage += TakeDamage;
        KeypadTask.taskDealDamage += TakeDamage;
        GameState.stationHealthUpdated += SetHealth;

        stationHealth = startHealth;
        halfHealth = startHealth / 2;
        quarterHealth = startHealth / 4;
    }


    private void OnDestroy()
    {
        ThreeWayLeverTask.taskDealDamage -= TakeDamage;
        TwoWayLeverTask.taskDealDamage -= TakeDamage;
        KeypadTask.taskDealDamage -= TakeDamage;
        GameState.stationHealthUpdated -= SetHealth;
    }

    //reduce the station health by x amount, and send it to the server
    //on certain values launch an event
    void TakeDamage(int amount)
    {
        Debug.Log("taking damage");
        stationHealth -= amount;
        stationTookDamage?.Invoke(stationHealth);

        if(stationHealth == halfHealth)
        {
            stationHalfWay?.Invoke();
        }
        if (stationHealth == quarterHealth)
        {
            stationThreeQuarterWay?.Invoke();
        }
    }

    //if we get a new health amount from the server, update the health
    void SetHealth(int health)
    {
        stationHealth = health;
        if (stationHealth == halfHealth)
        {
            stationHalfWay?.Invoke();
        }
        if (stationHealth == quarterHealth)
        {
            stationThreeQuarterWay?.Invoke();
        }
    }

    public int GetStationHealth()
    {
        return stationHealth;
    }
}
