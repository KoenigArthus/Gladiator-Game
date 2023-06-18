using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeManager))]
public class TimeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TimeManager timeManager = (TimeManager)target;

        timeManager.directionalLighting = timeManager.lightObject.GetComponent<Light>();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Time"))
        {
            timeManager.SetPreviousTime();
        }

        if (GUILayout.Button("Set Time"))
        {
            timeManager.SetTimeTo(timeManager.currentTime);
        }

        if (GUILayout.Button("Next Time"))
        {
            timeManager.SetNextTime();
        }

        GUILayout.EndHorizontal();

        base.OnInspectorGUI();


    }
}
