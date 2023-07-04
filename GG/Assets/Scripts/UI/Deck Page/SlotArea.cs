using System;
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
            string equipmentName = equipmentCard.equipmentIDName;
            if(equipmentCard.slot != null)
            {
                equipmentCard.slot.equipment = "none";
                equipmentCard.slot = null;
            }
            Deckbuilder.instance.RemoveFromDeckEntrie((CardSet)Enum.Parse(typeof(CardSet),equipmentName));
            Deckbuilder.instance.FillEquipmentEntrie(equipmentName);

            Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.LoadEquipmentPanel();
            Deckbuilder.instance.slotArea.SetActive(false);
            Destroy(dropped);
        }
    }
}
