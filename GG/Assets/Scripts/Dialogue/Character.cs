using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
    [SerializeField]
    private string characterName;

    private DialogueRunner dialogueRunner;
    private bool intrigger;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        intrigger = true;
        Debug.Log("entered " + characterName + " dialogue trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        intrigger = false;
        Debug.Log("left " + characterName + " dialogue trigger");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & intrigger)
        {
            if (dialogueRunner.IsDialogueRunning == false)
            {
                dialogueRunner.StartDialogue(characterName);
            }
        }


    }






}
