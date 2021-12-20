using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyManager : MonoBehaviour
{
    int id = 0;

    public int GetNewID()
    {
        id += 1;
        return id;
    }
}
