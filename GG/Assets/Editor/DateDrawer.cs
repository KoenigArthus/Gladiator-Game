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

        //Draw Label
        Color labelColor = ColorUtility.TryParseHtmlString("#b1fd59", out Color color) ? color : Color.green; // Specify the hexadecimal color value here
        string tooltipText = "A Date given in Day.Month.Year format";
        GUIContent labelContent = new GUIContent(label.ToString(), tooltipText);
        EditorGUI.LabelField(labelRect, labelContent, new GUIStyle(EditorStyles.label) { normal = { textColor = labelColor } });

        // Reset the color back to default
        GUI.color = Color.white; 

        // Calculate field width
        float fieldWidth = (position.width - labelWidth - 2f) / 8f;
        Rect fieldRect = new Rect(labelRect.xMax + 2f, position.y, fieldWidth, position.height);

        // Calculate total width of the fields and labels
        float totalWidth = fieldWidth * 4f + 2f * 10f;
        float xOffset = position.width - totalWidth - 66f;

        // Draw each field
        fieldRect.x = position.x + xOffset;
        fieldRect.x += 13f;
        EditorGUI.LabelField(fieldRect, "W");
        fieldRect.x += 16f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("weekday"), GUIContent.none);
        fieldRect.x += fieldWidth + 5f;
        EditorGUI.LabelField(fieldRect, "D");
        fieldRect.x += 13f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("day"), GUIContent.none);
        fieldRect.x += fieldWidth + 5f;
        EditorGUI.LabelField(fieldRect, "M");
        fieldRect.x += 16f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("month"), GUIContent.none);
        fieldRect.x += fieldWidth + 4f;
        EditorGUI.LabelField(fieldRect, "Y");
        fieldRect.x += 13f;
        EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("year"), GUIContent.none);



        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}

