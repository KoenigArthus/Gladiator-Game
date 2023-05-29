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
           //Equipment equipmentInfo = dropped.GetComponent<EquipmentCard>().equipment;
           //Deckbuilder.instance.RemoveFromDeckEntrie(CardLibrary.Cards.Where(x => x.Set == equipmentInfo.cardSet)); Deckbuilder.instance.LoadDeckPanel();
           ////Deckbuilder.instance.FillEquipmentEntrie(equipmentInfo);
           //Deckbuilder.instance.LoadDeckPanel();
           //Deckbuilder.instance.LoadEquipmentPanel();
        }
    }
}
