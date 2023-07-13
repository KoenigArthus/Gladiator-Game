using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public bool tutorialCompleted = false;
    public string startNode;

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                Debug.Log("Game resumed");
            }
            else if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Debug.Log("Game stopped");
            }
        }
        */
    }

    private void Start()
    {
        DialogueSystem.i.dialogueRunner.StartDialogue(startNode);
        // for (int i = 0; i < 10; i++)
        // {
        //     if (i % 2 == 0)
        //     {
        //         continue;
        //     }
        //     Debug.Log(i);
        // }
    }

}
