using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeManager))]
public class TimeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TimeManager timeManager = (TimeManager)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Date"))
        {
            timeManager.SetPreviousDate();
        }

        if (GUILayout.Button("Set Date"))
        {
            timeManager.SetCurrentDate(timeManager.currentDate);
        }

        if (GUILayout.Button("Next Date"))
        {
            timeManager.SetNextDate();
        }

        GUILayout.EndHorizontal();

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
