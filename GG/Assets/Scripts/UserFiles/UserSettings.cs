using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public class UserSettings : UserFile
{
    private Language language = Language.English;

    public UserSettings(string filename) : base(filename)
    {
    }

    public Language Language { get => language; set => language = value; }

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        SaveElement(doc, rootNode, "Language", language.ToString());
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        string language = LoadElement<string>(rootNode.SelectSingleNode("Language"));
        Enum.TryParse(language, true, out this.language);
    }
}