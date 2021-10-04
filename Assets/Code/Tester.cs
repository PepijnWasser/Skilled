using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private ConsoleRoom consoleRoomPrefab;
    public Doorway startDoor;
    public SecGenTester secGen;

    List<Doorway> xxxx = new List<Doorway>();

    private void Start()
    {
       // ConsoleRoom consoleRoom = Instantiate(consoleRoomPrefab, this.transform);
        Debug.Log(Random.Range(0, 1000));

        secGen.startdoor = startDoor;
        secGen.Initialize(LayerMask.GetMask("Room"));
        secGen.StartGeneration();
    }
}
