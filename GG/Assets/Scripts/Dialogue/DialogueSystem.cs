using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

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
    public Character speakingCharacter;
    private string speakerName;
    [SerializeField] private Image portaitImage;
    [SerializeField] private DialogueRunner dialogueRunner;
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
