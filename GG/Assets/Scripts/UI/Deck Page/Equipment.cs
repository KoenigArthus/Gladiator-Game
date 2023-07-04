using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentIDName;
    public Sprite sprite;
    public CardSet cardSet;
    public SlotType slotType;
}
