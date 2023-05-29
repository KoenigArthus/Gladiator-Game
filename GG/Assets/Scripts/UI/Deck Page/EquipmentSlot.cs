using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;


    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 & (eventData.pointerDrag.GetComponent<EquipmentCard>()?.equipment.slotType == slotType))
        {
            EquipmentCard equipmentCard = eventData.pointerDrag.GetComponent<EquipmentCard>();
            equipmentCard.afterDragParent = transform;
            Deckbuilder.instance.FillDeckEntrie(CardLibrary.Cards.Where(x => x.Set == equipmentCard.equipment.cardSet));
            Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.RemoveFromEquipmentEntrie(equipmentCard.equipment);
            Deckbuilder.instance.LoadEquipmentPanel();
        }
    }
}
