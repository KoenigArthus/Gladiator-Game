using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TimeBoolTable))]
public class TimeBoolTableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect a_Rect, SerializedProperty a_Property, GUIContent a_Label)
    {
        //Shorthand to make writing easier
        float lh = EditorGUIUtility.singleLineHeight;

        //Draw our label in the full rect (will self centre) but leave room at the end for the bools
        Rect labelRect = new Rect(a_Rect.x, a_Rect.y, a_Rect.width - lh * 6f, a_Rect.height);
        EditorGUI.LabelField(labelRect, a_Label);

        //This is where the bools will start on the x-axis
        float boolStartX = a_Rect.x + labelRect.width;
        //Initialise the bool rect to the top-left corner (1 line wide and tall)
        Rect boolRect = new Rect(boolStartX, a_Rect.y, lh, lh);
        //We want the bools to go left -> right then top -> bottom, so start with the y-axis as the outside loop
        for (int y = 0; y < 7; y++)
        {
            //Re-initialise the x-axis with every new row
            boolRect.x = boolStartX;
            for (int x = 0; x < 5; x++)
            {
                //Find the current index (starting at 1)
                int currentBool = x + y * 5 + 1;
                //Draw the bool as normal (without a label)
                EditorGUI.PropertyField(boolRect, a_Property.FindPropertyRelative("time" + currentBool), GUIContent.none);
                //Move to the next bool in the row
                boolRect.x += boolRect.height;
            }
            //Move to the next row of bools
            boolRect.y += boolRect.width;
        }
    }

    //We need to override the property height 
    public override float GetPropertyHeight(SerializedProperty a_Property, GUIContent a_Label)
    {
        return EditorGUIUtility.singleLineHeight * 7f;
    }
}
