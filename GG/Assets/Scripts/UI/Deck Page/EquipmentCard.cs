using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Scripts.UI.Deck_Page;

public class EquipmentCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [CustomAttributes.ReadOnly]
    public string equipmentIDName;
    public Image image;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardType;
    [CustomAttributes.ReadOnly] public SlotType slotType;
    [CustomAttributes.ReadOnly] public CardSet cardSet;
    [HideInInspector] public Transform afterDragParent;
    [CustomAttributes.ReadOnly] public EquipmentSlot slot;

    private void Start()
    {
        SetupCard();
    }

    private void SetupCard()
    {
        EquipmentInfo equipmentInfo = new EquipmentInfo(equipmentIDName);
        Debug.Log(equipmentInfo.Sprite);
        slotType = equipmentInfo.SlotType;
        cardSet = equipmentInfo.CardSet;
        image.sprite = equipmentInfo.Sprite;
        cardName.text = equipmentInfo.CardSet.ToString();
        cardType.text = equipmentInfo.SlotType.ToString();
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
