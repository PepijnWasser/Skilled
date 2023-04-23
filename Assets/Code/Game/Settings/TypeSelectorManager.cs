using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSelectorManager : MonoBehaviour
{
    public List<TypeSelector> typeSelectors;

    public void OnOpen()
    {
        Activate(typeSelectors[0]);
    }

    public void Activate(TypeSelector newSelector)
    {
        foreach(TypeSelector selector in typeSelectors)
        {
            selector.DeActivate();
        }
        newSelector.Activate();
    }
}
