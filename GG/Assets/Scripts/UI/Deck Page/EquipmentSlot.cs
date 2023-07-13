using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;
    public string equipment = "none";
   

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 & (eventData.pointerDrag.GetComponent<EquipmentCard>()?.slotType == slotType))
        {
            EquipmentCard equipmentCard = eventData.pointerDrag.GetComponent<EquipmentCard>();
            equipmentCard.afterDragParent = transform;
            Deckbuilder.instance.AddToDeckEntrie(CardLibrary.Cards.Where(x => x.Set == equipmentCard.cardSet));
            Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.RemoveFromEquipmentEntrie(equipmentCard.equipmentIDName);
            Deckbuilder.instance.LoadEquipmentPanel();
            equipment = equipmentCard.equipmentIDName;
            equipmentCard.slot = this;
        }
    }
}
