using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHealth : MonoBehaviour
{
    public int stationHealth = 100;

    private void Start()
    {
        TerminalTask.taskDealDamage += TakeDamage;
        KeypadTask.taskDealDamage += TakeDamage;
    }

    private void OnDestroy()
    {
        TerminalTask.taskDealDamage -= TakeDamage;
        KeypadTask.taskDealDamage -= TakeDamage;
    }

    void TakeDamage(int amount)
    {
        stationHealth -= amount;
    }
}
