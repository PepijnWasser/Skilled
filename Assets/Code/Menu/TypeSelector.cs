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

    Image instantiatedTypeContentImage;

    public void Activate()
    {
        selectedImage.enabled = true;
        instantiatedTypeContentImage = Instantiate(typeContentImage, typeContentField.transform.position, Quaternion.identity, typeContentField.transform);
    }

    public void DeActivate()
    {
        selectedImage.enabled = false;
        if(instantiatedTypeContentImage != null)
        {
            Destroy(instantiatedTypeContentImage);
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
