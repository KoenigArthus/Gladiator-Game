using System.Collections.Generic;
using System.Xml;

public class DialogSave : UserFile
{
    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    public Dictionary<string, int> values = new Dictionary<string, int>();

    public DialogSave() : base("Dialog")
    {
        flags.Add("Test", false);
        flags.Add("Hello, World!", false);
        flags.Add("Blub", false);

        values.Add("Life", 42);
        values.Add("Lucky", 7);
    }

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        XmlElement dictionaryNode = doc.CreateElement("Dialog");
        rootNode.AppendChild(dictionaryNode);

        SaveDictionary(doc, dictionaryNode, "Flags", flags);
        SaveDictionary(doc, dictionaryNode, "Values", values);
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        flags = LoadDictionary<string, bool>(rootNode.SelectSingleNode("Dialog/Flags"));
        values = LoadDictionary<string, int>(rootNode.SelectSingleNode("Dialog/Values"));
    }
}