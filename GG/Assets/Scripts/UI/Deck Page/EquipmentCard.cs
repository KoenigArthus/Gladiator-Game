using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class EquipmentCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [CustomAttributes.ReadOnly]
    public Equipment equipment;
    public EquipmentSlot slot;
    public Image image;
    public TMP_Text cardName;
    public TMP_Text cardType;

    [HideInInspector] public Transform afterDragParent;

    private void Start()
    {
        SetupCard(equipment.name);
    }

    private void SetupCard(string name)
    {



        image.sprite = 
        cardName.text = equipment.cardSet.ToString();
        cardType.text = equipment.slotType.ToString();
    }


    #region Drag Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        afterDragParent = transform.parent;
        transform.SetParent(transform.root.Find("UI Canvas"));
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        Deckbuilder.instance.slotArea.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(afterDragParent);
        image.raycastTarget = true;
        Deckbuilder.instance.slotArea.SetActive(false);
    }

    #endregion Drag Events

    public void OnPointerEnter(PointerEventData eventData)
    {
       // Deckbuilder.instance.tooltip.GetComponent<TooltipCard>().cardName = cardName;
       // Deckbuilder.instance.tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("bye");
    }



}
