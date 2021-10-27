using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRefresher : MonoBehaviour
{
    public GameObject objectToRefresh;

    public void Refresh()
    {
        StartCoroutine("RefreshObject");
    }

    IEnumerator RefreshObject()
    {
        objectToRefresh.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        objectToRefresh.SetActive(true);
        yield return null;

    }
}
