using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    [SerializeField] List<Character> characters;

    private void Awake()
    {
        // Find all objects with the type "Character" in the scene
        Character[] charactersAsArray = FindObjectsOfType<Character>();
        characters = charactersAsArray.ToList();

        foreach(Character character in characters)
        {
            character.gameObject.SetActive(false);
            
        
        }


    }
}
