/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Character character = (Character)target;

        base.OnInspectorGUI();


        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Reverse Spawn Table"))
            character.ToggleSpawnTable();

        GUILayout.EndHorizontal();


    }
}
*/