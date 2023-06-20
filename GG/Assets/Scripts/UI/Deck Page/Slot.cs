using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType 
{
    Hand,
    Head,
    Shoulder,
    Leg
}



public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 && eventData.pointerDrag.GetComponent<DraggableCard>() != null)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableCard draggableCard = dropped.GetComponent<DraggableCard>();
            draggableCard.afterDragParent = transform;
        }
    }
}
