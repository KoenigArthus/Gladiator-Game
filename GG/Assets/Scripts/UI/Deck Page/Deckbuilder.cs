using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Deckbuilder : MonoBehaviour
{
    [HideInInspector] public static Deckbuilder instance;
    public GameObject slotArea, tooltip;
    [Tooltip("check if the slots are in the right order: 0 = head, 1 = shoulder, 2 = leg, 3 = left, 4 = right")]
    public EquipmentSlot[] equippedSlots;
    [SerializeField] private Transform equipmentPanel, deckPanel;
    [SerializeField] private GameObject equipmentCardPreFab, inventoryCardPreFab;
    [SerializeField] private GameObject rowPreFabFourColumns, rowPreFabFiveColumns;
    public List<Transform> equipmentSlots = new List<Transform>();
    public List<Transform> deckSlots = new List<Transform>();
    public List<Equipment> equipmentCardEntries = new List<Equipment>();
    public List<string> deckCardEntries = new List<string>();
    private int minRowCount = 5;

    private void Awake()
    {
        //standart singleton pattern
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        tooltip.SetActive(false);

    }


    public void OnEnable()
    {
        LoadEquipmentPanel();
        LoadDeckPanel();
    }
    public void OnDisable()
    {
        ClearPanel(equipmentPanel, equipmentSlots);
        ClearPanel(deckPanel, deckSlots);
        SaveDeck();
        SaveEquipped();
        SaveEquipment();
        UserFile.SaveGame.Save();
    }

    private void LoadPanel(GameObject cardPreFab, Transform panel, List<Transform> slotList, List<string> entries, int columnCount)
    {
        ClearPanel(panel, slotList);


        int rowCount = Mathf.CeilToInt((float)entries.Count / (float)columnCount);

        if (rowCount < minRowCount)
            rowCount = minRowCount;

        switch (columnCount)
        {
            case 4: InstantiateRow(rowCount, rowPreFabFourColumns, panel); break;
            case 5: InstantiateRow(rowCount, rowPreFabFiveColumns, panel); break;
            default: Debug.LogWarning("Wrong column Count"); break;
        }

        //add slot references
        foreach (Transform row in panel)
        {
            foreach (Transform slot in row)
            {
                slotList.Add(slot);
            }
        }


        //Intantiate Cards into slots
        for (int i = 0; i < entries.Count; i++)
        {
            GameObject card = Instantiate(cardPreFab, slotList[i]);
            card.GetComponent<InventoryCard>().cardIDName = entries[i];
        }

    }
    private void LoadPanel(GameObject cardPreFab, Transform panel, List<Transform> slotList, List<Equipment> entries, int columnCount)
    {
        ClearPanel(panel, slotList);


        int rowCount = Mathf.CeilToInt((float)entries.Count / (float)columnCount);

        if (rowCount < minRowCount)
            rowCount = minRowCount;

        switch (columnCount)
        {
            case 4: InstantiateRow(rowCount, rowPreFabFourColumns, panel); break;
            case 5: InstantiateRow(rowCount, rowPreFabFiveColumns, panel); break;
            default: Debug.LogWarning("Wrong column Count"); break;
        }


        //add rows
        foreach (Transform row in panel)
        {
            foreach (Transform slot in row)
            {
                slotList.Add(slot);
            }
        }

        //Intantiate Cards into slots
        for (int i = 0; i < entries.Count; i++)
        {
            GameObject card = Instantiate(cardPreFab, slotList[i]);
            card.GetComponent<EquipmentCard>().equipment = entries[i];
        }
    }
    private void ClearPanel(Transform panel, List<Transform> slotList)
    {
        slotList.Clear();

        for (int i = panel.childCount - 1; i >= 0; i--)
        {
            GameObject gameObject = panel.GetChild(i).gameObject;
            DestroyImmediate(gameObject);
        }
    }
    private void InstantiateRow(int rowCount, GameObject rowPreFab, Transform panel)
    {
        for (int i = 0; i < rowCount; i++)
        {
            Instantiate(rowPreFab, panel);
        }
    }




    public void FillDeckEntrie(IEnumerable<CardInfo> cards)
    {
        foreach (CardInfo card in cards)
        {
            if (!deckCardEntries.Contains(card.Name))
                deckCardEntries.Add(card.Name);
        }
    }
    public void RemoveFromDeckEntrie(CardSet cardSet)
    {
        deckCardEntries = deckCardEntries.Where(x => CardLibrary.GetCardByName(x).Set != cardSet).ToList();
    }

    public void FillEquipmentEntrie(Equipment equipment)
    {
        if (equipmentCardEntries.Contains(equipment) == false)
            equipmentCardEntries.Add(equipment);
    }

    public void RemoveFromEquipmentEntrie(Equipment equipment)
    {
        if (equipmentCardEntries.Contains(equipment))
        {
            int indexToRemove = equipmentCardEntries.IndexOf(equipment);
            equipmentCardEntries.RemoveAt(indexToRemove);
        }
    }



    public void LoadEquipmentPanel()
    {
        LoadPanel(equipmentCardPreFab, equipmentPanel, equipmentSlots, equipmentCardEntries, 4);
    }
    public void LoadDeckPanel()
    {
        LoadPanel(inventoryCardPreFab, deckPanel, deckSlots, deckCardEntries, 5);
    }

    public void SaveDeck()
    {
        UserFile.SaveGame.DeckCardEntries = deckCardEntries.ToArray();
        //deckCardEntries = UserFile.SaveGame.DeckCardEntries.ToList(); 
        // UserFile.SaveGame.Save();|
    }

    public void SaveEquipment()
    {
        string[] equipmentCardEntriesAsString = equipmentCardEntries.ConvertAll(equipment => equipment.ToString()).ToArray();
        UserFile.SaveGame.EquipmentCardEntries = equipmentCardEntriesAsString;
    }

    public void SaveEquipped()
    {
        List<string> equipped = new List<string>();

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            if (equippedSlots[i].equipment != null & equippedSlots[i].equipment != "" )
            {
                equipped.Add(equippedSlots[i].equipment);
            }
            else
            {
                string s = "none";
                equipped.Add(s);
            }
        }

        Debug.Log(equipped.ToArray()[0] +","+ equipped.ToArray()[1] + "," + equipped.ToArray()[2] + "," + equipped.ToArray()[3] + "," + equipped.ToArray()[4]);

        UserFile.SaveGame.Equipped = equipped.ToArray();
        // UserFile.SaveGame.Save();
    }

}
