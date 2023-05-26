using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    [CustomAttributes.ReadOnly]
    public string cardIDName;
    public Image image;
    public TMP_Text cardName;
    public TMP_Text cardSet;
    public TMP_Text cardType;
    public TMP_Text cardDescription;

    private void Start()
    {
        //SetupCard(cardIDName);
    }



    private void SetupCard(string name)
    {
        CardLibrary.Setup();
        CardInfo cardInfo = CardLibrary.GetCardByName(name);
        //cardName.text = cardInfo.TranslatedName;
        cardSet.text = cardInfo.Set.ToString();
        cardType.text = cardInfo.Type.ToString();
        //cardDescription.text = cardInfo.TranslatedDescription;

    }



}
