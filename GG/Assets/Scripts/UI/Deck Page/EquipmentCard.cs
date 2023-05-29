using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class EquipmentCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [CustomAttributes.ReadOnly]
    public Equipment equipment;

    public Image image;
    public TMP_Text cardName;
    public TMP_Text cardType;
    [HideInInspector] public Transform afterDragParent;

    private void Start()
    {
        SetupCard();
    }

    private void SetupCard()
    {
        cardName.text = equipment.cardSet.ToString();
        cardType.text = equipment.slotType.ToString();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        afterDragParent = transform.parent;
        transform.SetParent(transform.root);
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







}
