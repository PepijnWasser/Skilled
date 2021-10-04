using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSetter : MonoBehaviour
{
    public int seed;
    // Start is called before the first frame update
    private void Awake()
    {
        Extensions.SetSeed(seed);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Random.state);
    }
}
