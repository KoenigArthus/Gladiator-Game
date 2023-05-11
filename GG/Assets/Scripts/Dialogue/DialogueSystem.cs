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
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject thoughtBox;
    public Camera cam;
    //Chacacter Fields
    [SerializeField] List<Character> characters;
    public Character speakingCharacter;
    private string speakerName;
    [SerializeField] private Image portaitImage;

    //Dialogue Fields
    public DialogueRunner dialogueRunner;
    [SerializeField] private LineViewCustom lineViewCustom;
    private VariableStorageCustom variableStorage;
    OptionView optionView;
    OptionsListView options;

    [SerializeField] private Dictionary<string, float> floatValues;
    [SerializeField] private Dictionary<string, string> stringValues;
    [SerializeField] private Dictionary<string, bool> boolValues;


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

        variableStorage = this.GetComponent<VariableStorageCustom>();

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            variableStorage.SecondCoins += 2;
            variableStorage.PlayerCoins += 1;
        }
    }


    // Is used in the Character Name View Script on the Dialogue System Game Object at OnNameUpdate()
    /// <summary>
    /// Changes the Dialogue Portrait to that of the referenced Character
    /// </summary>
    public void ChangeCharacterPortrait()
    {
        speakerName = lineViewCustom.currentLine.CharacterName;
        Sprite newSprite = null;

        // Chosing picture depending on localization of the name

        if (characters.Find(c => c.characterName == speakerName) != null)
        {
            speakingCharacter = characters.Find(c => c.characterName == speakerName);
            newSprite = speakingCharacter.portrait;
            ChangeToDialogueBox();
        }
        else if (characters.Find(c => c.englishCharacterName == speakerName) != null)
        {
            speakingCharacter = characters.Find(c => c.englishCharacterName == speakerName);
            newSprite = speakingCharacter.portrait;
            ChangeToDialogueBox();
        }
        else
        Debug.LogWarning("the Character has not been found!");

        // Check if sprite was successfully loaded
        if (newSprite != null)
        {
            // Change the sprite of the image component
            portaitImage.sprite = newSprite;
        }
    }

    // Is used in the Character Name View Script on the Dialogue System Game Object at OnNameNotPresent() 
    public void ChangeToThoughtBox()
    {
        portaitImage.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        thoughtBox.gameObject.SetActive(true);
    }

    public void ChangeToDialogueBox()
    {
        portaitImage.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);
        thoughtBox.gameObject.SetActive(false);
    }



    // Used in the Events of the Dialogue Runner on the Dialogue System Game Object at OnDialogueComplete()
    public void FinishDialogue()
    {
        portaitImage.gameObject.SetActive(false);
    }

    [YarnCommand("open_shop")]
    public static void OpenShop(string shopName)
    {
        Debug.Log(shopName);
    }

    [YarnCommand("add_card")]
    public static void UnlockCard(string cardName)
    {
        Debug.Log(cardName);
    }



}
