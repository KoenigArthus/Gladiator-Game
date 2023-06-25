using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TimeManager))]
public class TimeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TimeManager timeManager = (TimeManager)target;

        // Time & Date buttons
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Move back in Time"))
            timeManager.MoveBackInTime();

        if (GUILayout.Button("Set Date & Time"))
            timeManager.SetDateAndTime();

        if (GUILayout.Button("Move in Time"))
            timeManager.MoveInTime();

        GUILayout.EndHorizontal();


        // Date buttons
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Date"))
            timeManager.SetPreviousDate();

        if (GUILayout.Button("Set Date"))
            timeManager.SetDate(timeManager.currentDate);

        if (GUILayout.Button("Next Date"))
            timeManager.SetNextDate();

        GUILayout.EndHorizontal();

        //Time buttons
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Time"))
            timeManager.SetPreviousTime();

        if (GUILayout.Button("Set Time"))
            timeManager.SetTime(timeManager.currentTime);

        if (GUILayout.Button("Next Time"))
            timeManager.SetNextTime();

        GUILayout.EndHorizontal();

        // Reset buttons
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset Date"))
            timeManager.ResetDate();

        if (GUILayout.Button("Reset Time"))
            timeManager.ResetTime();

        if (GUILayout.Button("Reset Date & Time"))
            timeManager.ResetDateAndTime();

        GUILayout.EndHorizontal();
        

        base.OnInspectorGUI();


    }
}
