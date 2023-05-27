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
        ClearEntrie(deckCardEntries);
        ClearEntrie(equipmentCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Gladius), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Cassis), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Rete), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Scutum), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Scindo), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Trident), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Pugio), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Spartha), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Galerus), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Manica), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Ocrea), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Pilum), deckCardEntries);
        FillEntries(CardLibrary.Cards.Where(x => x.Set == CardSet.Parmula), deckCardEntries);
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

    private void ClearEntrie(List<string> entrieList)
    {
        entrieList.Clear();
    }

    private void FillEntries(IEnumerable<CardInfo> cards, List<string> entrieList)
    {
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
            GameObject card = Instantiate(cardPreFab, slotList[i]);
            Debug.Log(entries[i]);
            card.GetComponent<InventoryCard>().cardIDName = entries[i];
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
