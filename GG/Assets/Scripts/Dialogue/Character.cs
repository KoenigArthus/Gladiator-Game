using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
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
        UIManager.i.activeDialogueIcon.SetActive(true);
        Debug.Log("entered " + characterName + " dialogue trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        intrigger = false;
        UIManager.i.activeDialogueIcon.SetActive(false);
        Debug.Log("left " + characterName + " dialogue trigger");
    }


    private void Update()
    {
        if (intrigger)
        {
            recenterUIElement(UIManager.i.activeDialogueIcon, DialogueControllsOffset);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (dialogueRunner.IsDialogueRunning == false)
                {
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
        Vector3 pos = UIManager.i.camera.WorldToScreenPoint(transform.position + offset);

        if(uiObject.transform.position != pos)
            uiObject.transform.position = pos;
    }




}
