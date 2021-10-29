using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHealth : MonoBehaviour
{
    public int stationHealth = 100;

    public delegate void Damage(int health);
    public static event Damage updateStationHealth;

    public delegate void Destroyed();
    public static event Destroyed stationDestroyed;

    private void Awake()
    {
        ThreeWayLeverTask.taskDealDamage += TakeDamage;
        TwoWayLeverTask.taskDealDamage += TakeDamage;
        KeypadTask.taskDealDamage += TakeDamage;
        GameState.stationHealthUpdated += SetHealth;
    }

    private void Start()
    {
        Debug.Log(ServerConnectionData.isOwner);
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
        stationHealth -= amount;
        updateStationHealth?.Invoke(stationHealth);
        Debug.Log(stationHealth);

        if(stationHealth < 0)
        {
            if (ServerConnectionData.isOwner)
            {
                //TODO
            }
        }
    }

    void SetHealth(int health)
    {
        stationHealth = health;
    }
}
