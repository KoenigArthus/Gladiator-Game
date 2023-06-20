using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

public class SaveGame : UserFile
{
    #region Fields

    private string name;
    private int health = 20;
    private float gold = 0;
    private float[] equipmentEXP = new float[(int)CardSet.Health];
    private int nextOpponent = 0;
    private string[] deckCardEntries = new string[0];
    private string[] equipmentCardEntries = new string[0];
    private string[] equipped = new string[0];

    #endregion Fields

    public SaveGame(string filename) : base(filename)
    {
    }

    #region Properties

    public string Name { get => name; set => name = value; }
    public int Health { get => health; set => health = value; }
    public float Gold { get => gold; set => gold = value; }
    public float[] EquipmentEXP { get => equipmentEXP; set => equipmentEXP = value; }
    public int NextOpponent { get => nextOpponent; set => nextOpponent = value; }
    public string[] DeckCardEntries { get => deckCardEntries; set => deckCardEntries = value; }
    public string[] EquipmentCardEntries { get => equipmentCardEntries; set => equipmentCardEntries = value; }
    public string[] Equipped { get => equipped; set => equipped = value; }

    #endregion Properties

    #region Save/Load

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        //Name
        SaveElement(doc, rootNode, "Name", name);

        //Health
        SaveElement(doc, rootNode, "Health", health);

        //Gold
        SaveElement(doc, rootNode, "Gold", gold);

        //Equipment EXP
        SaveElement(doc, rootNode, "EquipmentEXP", string.Join(',', equipmentEXP));

        //Next Opponent
        SaveElement(doc, rootNode, "NextOpponent", nextOpponent);

        //Deck Entrie
        SaveElement(doc, rootNode, "DeckEntrie", string.Join(',', deckCardEntries));
        SaveElement(doc, rootNode, "EquipmentEntrie", string.Join(',', equipmentCardEntries));
        SaveElement(doc, rootNode, "Equipped", string.Join(',', equipped));
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        //Name
        name = LoadElement<string>(rootNode.SelectSingleNode("Name"));

        //Health
        health = LoadElement<int>(rootNode.SelectSingleNode("Health"));

        //Gold
        gold = LoadElement<float>(rootNode.SelectSingleNode("Gold"));

        //Equipment EXP
        {
            string data = LoadElement<string>(rootNode.SelectSingleNode("EquipmentEXP"));
            if (data != null && data.Length > 0)
                equipmentEXP = data.Split(',').Select(x => Convert.ToSingle(x)).ToArray();
        }

        //Next Opponent
        nextOpponent = LoadElement<int>(rootNode.SelectSingleNode("NextOpponent"));

        //Deck Entrie
        deckCardEntries = LoadElement<string>(rootNode.SelectSingleNode("DeckEntrie")).Split(',');
        equipmentCardEntries = LoadElement<string>(rootNode.SelectSingleNode("EquipmentEntrie")).Split(',');
        equipped = LoadElement<string>(rootNode.SelectSingleNode("Equipped")).Split(',');
    }

    #endregion Save/Load
}