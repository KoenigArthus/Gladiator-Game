using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

public class SaveGame : UserFile
{
    #region Fields

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private string name;

    #endregion Fields

    public SaveGame(string filename) : base(filename)
    {
        SetFlag("Init");
        SetFlag("Test");
        SetFlag("Hello, World!");
    }

    #region Properties

    public string Name { get => name; set => name = value; }

    #endregion Properties

    #region Flags

    public void SetFlag(string name, bool value = true)
    {
        if (flags.ContainsKey(name))
            flags[name] = value;
        else
            flags.Add(name, value);
    }

    public bool GetFlag(string name)
    {
        return flags.ContainsKey(name) && flags[name];
    }

    public string LogFlags()
    {
        return string.Join(" | ", flags.Select(x => $"{x.Key}: {x.Value}"));
    }

    #endregion Flags

    #region Save/Load

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        //Flags
        SaveDictionary(doc, rootNode, "Flags", flags);

        //Name
        SaveElement(doc, rootNode, "Name", name);
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        //Flags
        flags = LoadDictionary<string, bool>(rootNode.SelectSingleNode("Flags"));

        //Name
        name = LoadElement<string>(rootNode.SelectSingleNode("Name"));
    }

    #endregion Save/Load
}