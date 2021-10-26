using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TypeSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image hoverImage;
    public Image selectedImage;
    public Image typeContentField;
    public Image typeContentImage;

    public Button confirmButton;

    GameObject instantiatedTypeContentImage;

    private void Awake()
    {
        if (instantiatedTypeContentImage == null)
        {
            instantiatedTypeContentImage = Instantiate(typeContentImage, typeContentField.transform.position, Quaternion.identity, typeContentField.transform).gameObject;
        }
    }

    public void Activate()
    {
        selectedImage.enabled = true;
        if(instantiatedTypeContentImage == null)
        {
            instantiatedTypeContentImage = Instantiate(typeContentImage, typeContentField.transform.position, Quaternion.identity, typeContentField.transform).gameObject;
        }
        else
        {
            instantiatedTypeContentImage.SetActive(true);
        }
    }

    public void DeActivate()
    {
        selectedImage.enabled = false;
        if(instantiatedTypeContentImage != null)
        {
            instantiatedTypeContentImage.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.enabled = false;
    }
}
