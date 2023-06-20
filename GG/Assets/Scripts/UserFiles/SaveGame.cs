using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

public class SaveGame : UserFile
{
    #region Fields

    private string name;
    private int health = 20;
    private string[] deckCardEntries = new string[0];
    private string[] equipmentCardEntries = new string[0];
    private string[] equipt = new string[0];
    #endregion Fields

    public SaveGame(string filename) : base(filename)
    {
    }

    #region Properties

    public string Name { get => name; set => name = value; }
    public int Health { get => health; set => health = value; }
    public string[] DeckCardEntries { get => deckCardEntries; set => deckCardEntries = value; }
    public string[] EquipmentCardEntries { get => equipmentCardEntries; set => equipmentCardEntries = value; }
    public string[] Equipt { get => equipt; set => equipt = value; }
    #endregion Properties

    #region Save/Load

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        //Name
        SaveElement(doc, rootNode, "Name", name);

        //Health
        SaveElement(doc, rootNode, "Health", health);

        //Deck Entrie
        SaveElement(doc, rootNode, "DeckEntrie", string.Join(',', deckCardEntries));
        SaveElement(doc, rootNode, "EquipmentEntrie", string.Join(',', equipmentCardEntries));
        SaveElement(doc, rootNode, "Equipt", string.Join(',', equipt));
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        //Name
        name = LoadElement<string>(rootNode.SelectSingleNode("Name"));
        //Health
        health = LoadElement<int>(rootNode.SelectSingleNode("Health"));

        //Deck Entrie
        deckCardEntries = LoadElement<string>(rootNode.SelectSingleNode("DeckEntrie")).Split(',');
        equipmentCardEntries = LoadElement<string>(rootNode.SelectSingleNode("EquipmentEntrie")).Split(',');
        equipt = LoadElement<string>(rootNode.SelectSingleNode("Equipt")).Split(',');
    }

    #endregion Save/Load
}