using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalLayoutGroupContentSizeFitter : MonoBehaviour
{
    public List<GameObject> children;
    public GameObject parent;

    private void Start()
    {
        SetProperties();
    }

    public void SetProperties()
    {
        SetSizing();
        foreach (GameObject child in children)
        {
            if (child.GetComponent<VerticalLayoutGroupContentSizeFitter>() != null)
            {
                child.GetComponent<VerticalLayoutGroupContentSizeFitter>().SetProperties(this.gameObject);
            }
        }

        parent = transform.parent.gameObject;

        if (parent.GetComponent<VerticalLayoutGroupContentSizeFitter>() != null)
        {
            parent.GetComponent<VerticalLayoutGroupContentSizeFitter>().SetProperties(this.gameObject);
        }
    }

    public void SetProperties(GameObject objectToIgnore)
    {
        SetSizing();
        foreach (GameObject child in children)
        {
            if(child != objectToIgnore)
            {
                if (child.GetComponent<VerticalLayoutGroupContentSizeFitter>() != null)
                {
                    child.GetComponent<VerticalLayoutGroupContentSizeFitter>().SetProperties(this.gameObject);
                }
            }
        }

        parent = transform.parent.gameObject;

        if (parent.GetComponent<VerticalLayoutGroupContentSizeFitter>() != null)
        {
            if(parent != objectToIgnore)
            {
                parent.GetComponent<VerticalLayoutGroupContentSizeFitter>().SetProperties(this.gameObject);
            }
        }
    }

    void SetSizing()
    {
        children.Clear();
        List<RectTransform> childTransforms = new List<RectTransform>(GetComponentsInChildren<RectTransform>());
        foreach (Transform t in childTransforms)
        {
            if (t.parent == this.gameObject.transform)
            {
                children.Add(t.gameObject);
            }
        }

        float height = 0;
        float width = 0;
        foreach (GameObject child in children)
        {
            RectTransform childRect = child.GetComponent<RectTransform>();
            height += childRect.rect.height;
            if (width < childRect.rect.width)
            {
                width = childRect.rect.width;
            }
        }
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);

        float buildHeight = 0;

        foreach (GameObject g in children)
        {
            RectTransform childRect = g.GetComponent<RectTransform>();

            float y = -(buildHeight);
            childRect.anchoredPosition = new Vector3(childRect.rect.width / 2, y - childRect.rect.height / 2, 0);

            buildHeight += childRect.rect.height;
        }
    }
}
