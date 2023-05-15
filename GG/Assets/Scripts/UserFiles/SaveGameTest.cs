using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameTest : MonoBehaviour
{
    public InputField nameInput;
    private SaveGame save = new SaveGame("TestSave");
    private DialogSave dialog = new DialogSave();

    public void Save()
    {
        save.Name = nameInput.text;
        save.Save();

        dialog.Save();
    }

    public void Load()
    {
        save.Load();
        nameInput.text = save.Name;

        Debug.Log(save.LogFlags());
    }
}