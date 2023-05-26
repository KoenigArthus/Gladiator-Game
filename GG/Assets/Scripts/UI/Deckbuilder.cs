using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Deckbuilder : MonoBehaviour
{
    [SerializeField] private Transform equipmentPanel, deckPanel;
    [SerializeField] private GameObject dragableInventoryCardPreFab, inventoryCardPreFab;
    [SerializeField] private GameObject rowPreFabFourColumns, rowPreFabFiveColumns;
    public List<Transform> equipmentSlots = new List<Transform>();
    public List<Transform> deckSlots = new List<Transform>();
    public List<string> equipmentCardEntries = new List<string>();
    public List<string> deckCardEntries = new List<string>();

    public void OnEnable()
    {
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Trident), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Rete), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Trident),deckCardEntries);
        ClearPanel(equipmentPanel);
        ClearPanel(deckPanel);
        FillPanel(dragableInventoryCardPreFab, equipmentPanel, equipmentSlots, equipmentCardEntries, 4);
        FillPanel(inventoryCardPreFab, deckPanel, deckSlots, deckCardEntries, 5);
    }
    public void OnDisable()
    {
        ClearPanel(equipmentPanel);
        ClearPanel(deckPanel);
    }

    private void FillEntries(IEnumerable<CardInfo> cards, List<string> entrieList)
    {
        entrieList.Clear();
        foreach (CardInfo card in cards)
        {
            entrieList.Add(card.Name);
        }
    }

    private void FillPanel(GameObject cardPreFab, Transform panel, List<Transform> slotList, List<string> entries, int columnCount)
    {
        int rowCount = Mathf.CeilToInt((float)entries.Count / (float)columnCount);

        switch (columnCount)
        {
            case 4: InstantiateRow(rowCount, rowPreFabFourColumns, panel); break;
            case 5: InstantiateRow(rowCount, rowPreFabFiveColumns, panel); break;
            default: Debug.LogWarning("Wrong column Count"); break;
        }

        foreach (Transform row in panel)
        {
            foreach (Transform slot in row)
            {
                slotList.Add(slot);
            }
        }

        for (int i = 0; i < entries.Count; i++)
        {
            Instantiate(cardPreFab, slotList[i]);
        }
    }

    private void ClearPanel(Transform panel)
    {
        equipmentSlots.Clear();
        deckSlots.Clear();

        foreach (Transform row in panel)
        {
            Destroy(row.gameObject);
        }


    }
    
    private void InstantiateRow(int rowCount, GameObject rowPreFab, Transform panel)
    {
        for (int i = 0; i < rowCount; i++)
        {
            Instantiate(rowPreFab, panel);
        }
    }

}
