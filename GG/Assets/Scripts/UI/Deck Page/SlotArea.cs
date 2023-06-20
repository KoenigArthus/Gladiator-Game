using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;



public class SlotArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<EquipmentCard>() != null)
        {
            GameObject dropped = eventData.pointerDrag;
            EquipmentCard equipmentCard = dropped.GetComponent<EquipmentCard>();
            Equipment equipmentInfo = equipmentCard.equipment;
            if(equipmentCard.slot != null)
            {
                equipmentCard.slot.equipment = "none";
                equipmentCard.slot = null;
            }
            Deckbuilder.instance.RemoveFromDeckEntrie(equipmentInfo.cardSet);
            Deckbuilder.instance.FillEquipmentEntrie(equipmentInfo);

            Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.LoadEquipmentPanel();
            Deckbuilder.instance.slotArea.SetActive(false);
            Destroy(dropped);
        }
    }
}
