using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
    public Sprite portrait;
    public string characterName;
    public string englishCharacterName;
    public string startNode;
    [SerializeField]
    private Vector3 activeDialogueIconOffset;
    private bool intrigger;


    private void OnTriggerEnter(Collider other)
    {
        intrigger = true;
        DialogueSystem.i.activeDialogueIcon.SetActive(true);
        // Debug.Log("entered " + character.characterName + " dialogue trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        intrigger = false;
        DialogueSystem.i.activeDialogueIcon.SetActive(false);
        // Debug.Log("left " + character.characterName + " dialogue trigger");
    }


    private void Update()
    {
        if (intrigger)
        {
            recenterUIElement(DialogueSystem.i.activeDialogueIcon, activeDialogueIconOffset);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (DialogueSystem.i.dialogueRunner.IsDialogueRunning == false)
                {
                    DialogueSystem.i.dialogueRunner.StartDialogue(startNode);
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
