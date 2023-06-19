using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    [SerializeField] List<Character> characters;

    private void Awake()
    {
        // Find all objects with the type "Character" in the scene
        Character[] characterObjects = FindObjectsOfType<Character>();
    }
}
