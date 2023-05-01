using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public LineView lineView;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueRunner.IsDialogueRunning == false)
            {
                dialogueRunner.StartDialogue("Start");
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            lineView.OnContinueClicked();
        }
    }






}
