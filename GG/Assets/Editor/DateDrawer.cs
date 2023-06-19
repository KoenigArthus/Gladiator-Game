using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Date))]
public class DateDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate label width
        float labelWidth = EditorGUIUtility.labelWidth;
        Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);

        // Draw the label
        EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

        // Calculate field width
        float fieldWidth = (position.width - labelWidth - 2f) / 3f;
        Rect fieldRect = new Rect(labelRect.xMax + 2f, position.y, fieldWidth, position.height);

        // Calculate total width of the fields and labels
        float totalWidth = fieldWidth * 3f + 2f * 10f;
        float xOffset = position.width - totalWidth + 8f;

        // Draw each field
        fieldRect.x = position.x + xOffset;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("date1"), GUIContent.none);
        fieldRect.x += fieldWidth + 1f;
        EditorGUI.LabelField(fieldRect, ".");
        fieldRect.x += 5f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("date2"), GUIContent.none);
        fieldRect.x += fieldWidth + 1f;
        EditorGUI.LabelField(fieldRect, ".");
        fieldRect.x += 5f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("date3"), GUIContent.none);



        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}

