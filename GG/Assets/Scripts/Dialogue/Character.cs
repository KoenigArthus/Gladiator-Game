using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
    public Sprite portrait;
    [SerializeField]
    private string characterName;
    [SerializeField]
    private Vector3 DialogueControllsOffset;
    private DialogueRunner dialogueRunner;
    private bool intrigger;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        intrigger = true;
        DialogueSystem.i.activeDialogueIcon.SetActive(true);
       // Debug.Log("entered " + characterName + " dialogue trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        intrigger = false;
        DialogueSystem.i.activeDialogueIcon.SetActive(false);
       // Debug.Log("left " + characterName + " dialogue trigger");
    }


    private void Update()
    {
        if (intrigger)
        {
            recenterUIElement(DialogueSystem.i.activeDialogueIcon, DialogueControllsOffset);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (dialogueRunner.IsDialogueRunning == false)
                {
                    DialogueSystem.i.speakingCharacter = this;
                    dialogueRunner.StartDialogue(characterName);
                }
            }
        }

    }

    /// <summary>
    /// recenters the UI Element relative to the character + an offset
    /// must be used in an Update function to work properly
    /// </summary>
    /// <param name="uiObject"></param>
    private void recenterUIElement(GameObject uiObject, Vector3 offset)
    {
        Vector3 pos = DialogueSystem.i.cam.WorldToScreenPoint(transform.position + offset);

        if(uiObject.transform.position != pos)
            uiObject.transform.position = pos;
    }




}
