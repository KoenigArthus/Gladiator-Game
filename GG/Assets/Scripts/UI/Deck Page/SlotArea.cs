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
            Equipment equipmentInfo = dropped.GetComponent<EquipmentCard>().equipment;
            Deckbuilder.instance.RemoveFromDeckEntrie(CardLibrary.Cards.Where(x => x.Set == equipmentInfo.cardSet)); Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.FillEquipmentEntrie(equipmentInfo);
            Deckbuilder.instance.LoadDeckPanel();
            Deckbuilder.instance.LoadEquipmentPanel();
            Deckbuilder.instance.slotArea.SetActive(false);
            Destroy(dropped);
        }
    }
}
