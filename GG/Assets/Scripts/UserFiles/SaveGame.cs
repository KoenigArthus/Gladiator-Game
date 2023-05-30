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


    #endregion Fields

    public SaveGame(string filename) : base(filename)
    {
    }

    #region Properties

    public string Name { get => name; set => name = value; }
    public int Health { get => health; set => health = value; }
    public string[] DeckCardEntries { get => deckCardEntries; set => deckCardEntries = value; }

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
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        //Name
        name = LoadElement<string>(rootNode.SelectSingleNode("Name"));
        //Health
        health = LoadElement<int>(rootNode.SelectSingleNode("Health"));

        //Deck Entrie
        deckCardEntries = LoadElement<string>(rootNode.SelectSingleNode("DeckEntrie")).Split(',');
    }

    #endregion Save/Load
}