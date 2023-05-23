using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UserFile
{
    #region Fields

    private static SaveGame saveGame = new SaveGame("default");
    private static UserSettings settings = new UserSettings("settings");

    private string filename;

    #endregion Fields

    public UserFile(string filename)
    {
        this.filename = filename;

        if (File.Exists(Filepath))
            Load();
        else
            Save();
    }

    #region Properties

    public string Filepath => $"UserFiles\\Save\\{filename}.xml";

    public static SaveGame SaveGame => saveGame;
    public static UserSettings Settings => settings;

    #endregion Properties

    #region Save/Load

    public void Save()
    {
        XmlDocument doc = new XmlDocument();
        XmlElement rootNode = doc.CreateElement("UserFile");
        doc.AppendChild(rootNode);

        DoSave(doc, rootNode);

        Directory.CreateDirectory("UserFiles\\Save");
        doc.Save(Filepath);
    }

    protected abstract void DoSave(XmlDocument doc, XmlElement rootNode);

    protected void SaveElement(XmlDocument doc, XmlElement targetNode, string nodeName, object data, params (string, object)[] attributes)
    {
        XmlElement node = doc.CreateElement(nodeName);
        if (data != null)
            node.InnerText = data.ToString();
        for (int i = 0; i < attributes.Length; i++)
            node.SetAttribute(attributes[i].Item1, attributes[i].Item2.ToString());
        targetNode.AppendChild(node);
    }

    protected void SaveDictionary<TKey, TValue>(XmlDocument doc, XmlElement targetNode, string nodeName, Dictionary<TKey, TValue> data)
    {
        XmlElement dictionaryNode = doc.CreateElement(nodeName);

        foreach (var item in data)
        {
            SaveElement(doc, dictionaryNode, "Entry", null, ("Key", item.Key), ("Value", item.Value));
        }

        targetNode.AppendChild(dictionaryNode);
    }

    public void Load()
    {
        if (!File.Exists(Filepath))
            return;

        XmlDocument doc = new XmlDocument();
        doc.Load(Filepath);
        XmlNode rootNode = doc.FirstChild;

        DoLoad(rootNode);
    }

    protected abstract void DoLoad(XmlNode rootNode);

    protected T LoadElement<T>(XmlNode loadNode, string attributeName = null)
    {
        string data;

        if (attributeName != null)
            data = loadNode.Attributes[attributeName].Value;
        else
            data = loadNode.InnerText;

        return (T)Convert.ChangeType(data, typeof(T));
    }

    protected Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>(XmlNode loadNode)
    {
        Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        var nodes = loadNode.SelectNodes("Entry");
        foreach (XmlNode node in nodes)
        {
            dictionary.Add(LoadElement<TKey>(node, "Key"), LoadElement<TValue>(node, "Value"));
        }
        return dictionary;
    }

    #endregion Save/Load
}