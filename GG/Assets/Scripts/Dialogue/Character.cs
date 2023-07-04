using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite portrait;
    public string characterName;
    public string englishCharacterName;
    public string startNode;
    [SerializeField] private Vector3 activeDialogueIconOffset = new Vector3(0,1.9f,0);
    public Date spawnDate;
    public TimeBoolTable spawnTable;
    public Date killDate;

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
        if (intrigger && DialogueSystem.i != null)
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

        if (uiObject.transform.position != pos)
            uiObject.transform.position = pos;
    }

    public void ToggleSpawnTable()
    {
        CustomUtility.ToggleBool(ref spawnTable.time1);
        CustomUtility.ToggleBool(ref spawnTable.time2);
        CustomUtility.ToggleBool(ref spawnTable.time3);
        CustomUtility.ToggleBool(ref spawnTable.time4);
        CustomUtility.ToggleBool(ref spawnTable.time5);
        CustomUtility.ToggleBool(ref spawnTable.time6);
        CustomUtility.ToggleBool(ref spawnTable.time7);
        CustomUtility.ToggleBool(ref spawnTable.time8);
        CustomUtility.ToggleBool(ref spawnTable.time9);

        CustomUtility.ToggleBool(ref spawnTable.time10);
        CustomUtility.ToggleBool(ref spawnTable.time11);
        CustomUtility.ToggleBool(ref spawnTable.time12);
        CustomUtility.ToggleBool(ref spawnTable.time13);
        CustomUtility.ToggleBool(ref spawnTable.time14);
        CustomUtility.ToggleBool(ref spawnTable.time15);
        CustomUtility.ToggleBool(ref spawnTable.time16);
        CustomUtility.ToggleBool(ref spawnTable.time17);
        CustomUtility.ToggleBool(ref spawnTable.time18);
        CustomUtility.ToggleBool(ref spawnTable.time19);

        CustomUtility.ToggleBool(ref spawnTable.time20);
        CustomUtility.ToggleBool(ref spawnTable.time21);
        CustomUtility.ToggleBool(ref spawnTable.time22);
        CustomUtility.ToggleBool(ref spawnTable.time23);
        CustomUtility.ToggleBool(ref spawnTable.time24);
        CustomUtility.ToggleBool(ref spawnTable.time25);
        CustomUtility.ToggleBool(ref spawnTable.time26);
        CustomUtility.ToggleBool(ref spawnTable.time27);
        CustomUtility.ToggleBool(ref spawnTable.time28);
        CustomUtility.ToggleBool(ref spawnTable.time29);

        CustomUtility.ToggleBool(ref spawnTable.time30);
        CustomUtility.ToggleBool(ref spawnTable.time31);
        CustomUtility.ToggleBool(ref spawnTable.time32);
        CustomUtility.ToggleBool(ref spawnTable.time33);
        CustomUtility.ToggleBool(ref spawnTable.time34);
        CustomUtility.ToggleBool(ref spawnTable.time35);


    }




}
