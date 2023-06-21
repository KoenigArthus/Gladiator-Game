using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingLocationDecider : MonoBehaviour
{
    public GameObject[] fightingLocations;

    private void Awake()
    {
        int index = UserFile.SaveGame.FightingLocation;
        SetAllFalse();
        if (index >= fightingLocations.Length)
        {
            fightingLocations[0].SetActive(true);
        }
        else
        {
            fightingLocations[index].SetActive(true);
        }
    }

    private void SetAllFalse()
    {
        for( int i = 0; i < fightingLocations.Length; i++)
        {
            fightingLocations[i].SetActive(false);
        }
    }
}
