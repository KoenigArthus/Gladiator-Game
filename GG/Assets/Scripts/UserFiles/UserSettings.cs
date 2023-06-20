using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public class UserSettings : UserFile
{
    private Language language = Language.English;
    private bool disableRomanNumbursOnCards = false;
    private bool disableRomanNumbursOnDice = false;

    public UserSettings(string filename) : base(filename)
    {
    }

    public Language Language { get => language; set => language = value; }
    public bool DisableRomanNumbursOnDice { get => disableRomanNumbursOnDice; set => disableRomanNumbursOnDice = value; }

    protected override void DoSave(XmlDocument doc, XmlElement rootNode)
    {
        SaveElement(doc, rootNode, "Language", language.ToString());
        SaveElement(doc, rootNode, "DisableRomanNumburs", null, ("OnCards", disableRomanNumbursOnCards.ToString()), ("OnDice", disableRomanNumbursOnDice.ToString()));
    }

    protected override void DoLoad(XmlNode rootNode)
    {
        string language = LoadElement<string>(rootNode.SelectSingleNode("Language"));
        Enum.TryParse(language, true, out this.language);

        XmlNode disableRomanNumbers = rootNode.SelectSingleNode("DisableRomanNumburs");
        disableRomanNumbursOnCards = LoadElement<bool>(disableRomanNumbers, "OnCards");
        disableRomanNumbursOnDice = LoadElement<bool>(disableRomanNumbers, "OnDice");
    }
}