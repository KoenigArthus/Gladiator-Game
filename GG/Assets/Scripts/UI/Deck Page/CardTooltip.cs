using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
      //  Debug.Log("over");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // Debug.Log("bye");
    }
}
