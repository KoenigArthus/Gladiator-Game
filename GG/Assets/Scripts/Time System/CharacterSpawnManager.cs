using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TimeManager))]
public class CharacterSpawnManager : MonoBehaviour
{
    [SerializeField] Character[] characters;
    TimeManager timeManager;



    private void Awake()
    {
        timeManager = GetComponent<TimeManager>();

        // Find all objects with the type "Character" in the scene
        characters = FindObjectsOfType<Character>();

        foreach (Character character in characters)
        {
            character.gameObject.SetActive(false);
        }


    }

    private void Start()
    {
       ScheduleCharacters();
    }

    public void ScheduleCharacters()
    {
        int weekday = timeManager.currentDate.weekday;
        int time = timeManager.currentTime;

        for (int i = 0; i < characters.Length; i++)
            characters[i].gameObject.SetActive(IsActive(weekday, time, characters[i]));

    }

    private bool IsActive(int weekdayIndex, int timeIndex, Character character)
    {
        bool doActive = false;

        if (IsInSpawnTime(character.spawnDate, timeManager.passedDays, character.killDate))
        {
              switch (weekdayIndex)
            {
                case 0: // Monday
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time1; break;    // Sunrise
                        case 1: doActive = character.spawnTable.time2; break;    // Morning
                        case 2: doActive = character.spawnTable.time3; break;    // Afternoon
                        case 3: doActive = character.spawnTable.time4; break;    // Evening
                        case 4: doActive = character.spawnTable.time5; break;    // Night
                    }
                    break;

                case 1: // Tuesday
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time6; break;    // Sunrise
                        case 1: doActive = character.spawnTable.time7; break;    // Morning
                        case 2: doActive = character.spawnTable.time8; break;    // Afternoon
                        case 3: doActive = character.spawnTable.time9; break;    // Evening
                        case 4: doActive = character.spawnTable.time10; break;   // Night
                    }
                    break;

                // Repeat the switch cases for Wednesday to Sunday

                // Wednesday
                case 2:
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time11; break;   // Sunrise
                        case 1: doActive = character.spawnTable.time12; break;   // Morning
                        case 2: doActive = character.spawnTable.time13; break;   // Afternoon
                        case 3: doActive = character.spawnTable.time14; break;   // Evening
                        case 4: doActive = character.spawnTable.time15; break;   // Night
                    }
                    break;

                // Thursday
                case 3:
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time16; break;   // Sunrise
                        case 1: doActive = character.spawnTable.time17; break;   // Morning
                        case 2: doActive = character.spawnTable.time18; break;   // Afternoon
                        case 3: doActive = character.spawnTable.time19; break;   // Evening
                        case 4: doActive = character.spawnTable.time20; break;   // Night
                    }
                    break;

                // Friday
                case 4:
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time21; break;   // Sunrise
                        case 1: doActive = character.spawnTable.time22; break;   // Morning
                        case 2: doActive = character.spawnTable.time23; break;   // Afternoon
                        case 3: doActive = character.spawnTable.time24; break;   // Evening
                        case 4: doActive = character.spawnTable.time25; break;   // Night
                    }
                    break;

                // Saturday
                case 5:
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time26; break;   // Sunrise
                        case 1: doActive = character.spawnTable.time27; break;   // Morning
                        case 2: doActive = character.spawnTable.time28; break;   // Afternoon
                        case 3: doActive = character.spawnTable.time29; break;   // Evening
                        case 4: doActive = character.spawnTable.time30; break;   // Night
                    }
                    break;

                // Sunday
                case 6:
                    switch (timeIndex)
                    {
                        case 0: doActive = character.spawnTable.time31; break;   // Sunrise
                        case 1: doActive = character.spawnTable.time32; break;   // Morning
                        case 2: doActive = character.spawnTable.time33; break;   // Afternoon
                        case 3: doActive = character.spawnTable.time34; break;   // Evening
                        case 4: doActive = character.spawnTable.time35; break;   // Night
                    }
                    break;
            }
        }

        return doActive;
    }



    /// <param name="spawnDate">The date representing the spawn date.</param>
    /// <param name="passedDays">The date representing the current date.</param>
    /// <param name="killDate">The date representing the kill date.</param>
    /// <returns> true if the date in the <paramref name="passedDays"/> is between the two dates; otherwise, false.</returns>
    private bool IsInSpawnTime(Date spawnDate, int passedDays, Date killDate)
    {
        Debug.Log(CustomUtility.ToPassedDays(spawnDate) + ", " + passedDays + ", " + CustomUtility.ToPassedDays(killDate));
            
        if (CustomUtility.ToPassedDays(spawnDate) <= passedDays && passedDays < CustomUtility.ToPassedDays(killDate))
        {
            return true;
        }
        return false;
    }







}
