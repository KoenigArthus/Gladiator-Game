using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

/// <summary>
/// The Dialogue System is controlling various Dialogue related interactions
/// It also holds important references and acts as a singleton
/// It also servese functions to be used by yarn files and other Dialogue related classes
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    /// <summary>
    /// the singleton instance of the DialogueSystem
    /// </summary>
    public static DialogueSystem i;
    /// <summary>
    /// the icon displayed above a characters head when able to speak to
    /// </summary>
    public GameObject activeDialogueIcon;
    public Camera cam;
    //Chacacter Fields
    [SerializeField] List<Character> characters;
    public Character speakingCharacter;
    private string speakerName;
    [SerializeField] private Image portaitImage;

    //Dialogue Fields
    public DialogueRunner dialogueRunner;
    [SerializeField] private LineViewCustom lineViewCustom;
    OptionView optionView;
    OptionsListView options;

    private void Awake()
    {
        // default singleton pattern
        if (i == null)
        {
            DontDestroyOnLoad(gameObject);
            i = this;
        }
        else if (i != this)
        {
            Destroy(gameObject);
        }
    }


    public void ChangeCharacterPortrait()
    {
        speakerName = lineViewCustom.currentLine.CharacterName;
        speakingCharacter = characters.Find(c => c.characterName == speakerName);
        Debug.Log(speakingCharacter.characterName);
        Sprite newSprite = speakingCharacter.portrait;

        // Check if sprite was successfully loaded
        if (newSprite != null)
        {
            // Change the sprite of the image component
            portaitImage.sprite = newSprite;
        }
    }

    public void InitializeDialogue()
    {
        portaitImage.gameObject.SetActive(true);
    }
    public void FinishDialogue()
    {
        portaitImage.gameObject.SetActive(false);
    }

}
