using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCJScreen : MonoBehaviour
{
    public GameObject CJScreenprefab;

    public void CreateCJScreenItem()
    {
        Instantiate(CJScreenprefab);
        Destroy(this.gameObject);
    }
}
