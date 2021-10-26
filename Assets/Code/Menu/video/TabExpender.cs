using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabExpender : MonoBehaviour
{
    public Image imagePlace;

    public Sprite openedSprite;
    public Sprite closedSprite;

    public GameObject content;

    public List<GameObject> children = new List<GameObject>();

    bool opened = false;

    private void Start()
    {
        List<Transform> childTransforms = new List<Transform>(content.GetComponentsInChildren<Transform>());
        foreach(Transform t in childTransforms)
        {
            children.Add(t.gameObject);
        }

        Close();
    }

    public void switchSelection()
    {
        if (opened)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    void Open()
    {
        opened = true;
        imagePlace.sprite = openedSprite;
        foreach(GameObject child in children)
        {
            child.SetActive(true);
        }
    }

    void Close()
    {
        opened = false;
        imagePlace.sprite = closedSprite;

        foreach (GameObject child in children)
        {
            child.SetActive(false);
        }
    }
}
