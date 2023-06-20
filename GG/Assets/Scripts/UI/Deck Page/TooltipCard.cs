using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipCard : MonoBehaviour
{
    public Image image;
    [CustomAttributes.ReadOnly]public string cardIDName;
    public TMP_Text cardName;
    public TMP_Text cardSet;
    public TMP_Text cardType;
    public TMP_Text cardDescription;


    private void OnEnable()
    {
        CardLibrary.Setup();
        CardInfo cardInfo = CardLibrary.GetCardByName(cardIDName);
        cardName.text = cardInfo.TranslatedName;
        cardSet.text = cardInfo.Set.ToString();
        cardType.text = cardInfo.Type.ToString();
        image.sprite = cardInfo.Sprite;
        cardDescription.text = cardInfo.TranslatedDescription;


    }

    private void OnDisable()
    {

    }











}
