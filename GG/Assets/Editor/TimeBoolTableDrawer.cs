using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TimeBoolTable))]
public class TimeBoolTableDrawer : PropertyDrawer
{
    private string[] weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    private const int NumColumns = 5;
    private const int NumRows = 7;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Shorthand to make writing easier
        float lineHeight = EditorGUIUtility.singleLineHeight;

        // Calculate the width of each bool cell, accounting for spacing
        float cellWidth = (position.width - lineHeight * NumColumns) / NumColumns;

        // Calculate the initial position for the bool cells
        Rect cellRect = new Rect(position.x, position.y, cellWidth, lineHeight);

        // Disable the default spacing between PropertyFields
        int originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Loop through rows and columns to draw the bool table
        for (int row = 0; row < NumRows; row++)
        {
            // Draw the weekday label for the current row
            Rect rowLabelRect = new Rect(position.x, cellRect.y, 100f, lineHeight);
            EditorGUI.LabelField(rowLabelRect, weekdays[row]);

            // Move to the position of the first cell in the row
            cellRect.x = position.x + 100f;

            for (int column = 0; column < NumColumns; column++)
            {
                // Calculate the index of the current bool (starting at 1)
                int currentIndex = column + row * NumColumns + 1;

                // Draw the bool as normal (without a label) with zero spacing
                EditorGUI.PropertyField(cellRect, property.FindPropertyRelative("time" + currentIndex), GUIContent.none);

                // Move to the next cell in the row
                cellRect.x += cellRect.width;
            }

            // Move to the next row of cells
            cellRect.x = position.x;
            cellRect.y += lineHeight;
        }

        // Restore the original indent level
        EditorGUI.indentLevel = originalIndentLevel;
    }

    // We need to override the property height - let's make it 7 lines (a regular property will be 1 line high)
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * NumRows;
    }
}
