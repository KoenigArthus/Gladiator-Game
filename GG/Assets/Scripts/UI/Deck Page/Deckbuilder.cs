using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public enum PanelType
{
    Deck,
    Equipment
}


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

        UserFile.SaveGame.DeckCardEntries = new string[0];
        UserFile.SaveGame.EquipmentCardEntries = new string[] { "Cassis", "Doru", "Galerus", "Gladius", "Manica", "Ocrea", "Parmula", "Pugio", "Rete", "Scindo", "Scutum", "Trident" };

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
        //SaveDeck();
        SaveEquipped();
        //SaveEquipment();
        UserFile.SaveGame.Save();
    }


    #region Panel
    // this should not be needed with the new system for equipments
    /*  private void LoadPanel(GameObject cardPreFab, Transform panel, List<Transform> slotList, List<Equipment> entries, int columnCount)
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
      }*/
    private void LoadPanel(GameObject cardPreFab, Transform panel, List<Transform> slotList, List<string> entries, int columnCount, PanelType panelType)
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

        switch (panelType)
        {
            case PanelType.Deck:
                //Intantiate Cards into slots
                for (int i = 0; i < entries.Count; i++)
                {
                    GameObject card = Instantiate(cardPreFab, slotList[i]);
                    card.GetComponent<InventoryCard>().cardIDName = entries[i];
                }
                break;

            case PanelType.Equipment:
                //Intantiate Cards into slots
                for (int i = 0; i < entries.Count; i++)
                {
                    GameObject card = Instantiate(cardPreFab, slotList[i]);
                    card.GetComponent<EquipmentCard>().equipmentIDName = entries[i];
                }
                break;

            default:
                Debug.LogWarning("wrong panel ?");
                break;

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
    public void LoadEquipmentPanel()
    {
        LoadPanel(equipmentCardPreFab, equipmentPanel, equipmentSlots, UserFile.SaveGame.EquipmentCardEntries.ToList(), 4, PanelType.Equipment);
    }
    public void LoadDeckPanel()
    {
        LoadPanel(inventoryCardPreFab, deckPanel, deckSlots, UserFile.SaveGame.DeckCardEntries.ToList(), 5, PanelType.Deck);
    }
    #endregion Panel


    #region Deck
    public void FillDeckEntrie(IEnumerable<CardInfo> cards)
    {
        List<string> temporaryList = UserFile.SaveGame.DeckCardEntries.ToList();

        foreach (CardInfo card in cards)
        {
            if (!temporaryList.Contains(card.Name))
                temporaryList.Add(card.Name);
        }

        UserFile.SaveGame.DeckCardEntries = temporaryList.ToArray();

    }
    public void AddToDeckEntrie(string cardName)
    {
        List<string> temporaryList = UserFile.SaveGame.DeckCardEntries.ToList();

        if (CardLibrary.GetCardByName(cardName) != null)
            temporaryList.Add(CardLibrary.GetCardByName(cardName).Name);
        else
            Debug.LogWarning(cardName + " could not be found!");
 
        UserFile.SaveGame.DeckCardEntries = temporaryList.ToArray();

    }


    public void RemoveFromDeckEntrie(CardSet cardSet)
    {
        UserFile.SaveGame.DeckCardEntries = UserFile.SaveGame.DeckCardEntries.Where(x => CardLibrary.GetCardByName(x).Set != cardSet).ToArray();
    }
    public void RemoveFromDeckEntrie(string cardname)
    {
        List<string> temporaryList = UserFile.SaveGame.DeckCardEntries.ToList();
        temporaryList.Remove(cardname);
        UserFile.SaveGame.DeckCardEntries = temporaryList.ToArray();
    }


    [Obsolete("UserFile.SaveGame.DeckCardEntries is modified directly", true)]
    public void SaveDeck()
    {

       // UserFile.SaveGame.DeckCardEntries = deckCardEntries.ToArray();
        //deckCardEntries = UserFile.SaveGame.DeckCardEntries.ToList(); 
        // UserFile.SaveGame.Save();|
    }
    #endregion Deck

    #region Equipment
    public void FillEquipmentEntrie(string equipmentname)
    {
        List<string> temporaryList = UserFile.SaveGame.EquipmentCardEntries.ToList();

        if (temporaryList.Contains(equipmentname) == false)
            temporaryList.Add(equipmentname);

        UserFile.SaveGame.EquipmentCardEntries = temporaryList.ToArray();
    }
    public void RemoveFromEquipmentEntrie(string equipmentname)
    {
        List<string> temporaryList = UserFile.SaveGame.EquipmentCardEntries.ToList();
        temporaryList.Remove(equipmentname);
        UserFile.SaveGame.EquipmentCardEntries = temporaryList.ToArray();
    }
    [Obsolete("UserFile.SaveGame.EquipmentEntries is modified directly", true)]
    public void SaveEquipment()
    {
        //string[] equipmentCardEntriesAsString = equipmentCardEntries.ConvertAll(equipment => equipment.ToString()).ToArray();
       // UserFile.SaveGame.EquipmentCardEntries = equipmentCardEntriesAsString;
    }

    #endregion Equipment


    #region Equipped
    public void SaveEquipped()
    {
        List<string> equipped = new List<string>();

        for (int i = 0; i < equippedSlots.Length; i++)
        {
            if (equippedSlots[i].equipment != null & equippedSlots[i].equipment != "")
            {
                equipped.Add(equippedSlots[i].equipment);
            }
            else
            {
                string s = "none";
                equipped.Add(s);
            }
        }

        Debug.Log(equipped.ToArray()[0] + "," + equipped.ToArray()[1] + "," + equipped.ToArray()[2] + "," + equipped.ToArray()[3] + "," + equipped.ToArray()[4]);

        UserFile.SaveGame.Equipped = equipped.ToArray();
        // UserFile.SaveGame.Save();
    }
    #endregion Equipped















}
