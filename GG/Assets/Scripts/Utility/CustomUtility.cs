using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CustomUtility
{
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static void ReplaceSpacesWithUnderscoresInplace(ref string text)
    {
        // Replace all spaces in the given string with underscores in place.
        text = text.Replace(' ', '_');
    }

    public static int ToPassedDays(Date date)
    {
        // Calculate the total number of days passed since the starting date
        int yearsPassed = date.year - 1;
        int monthsPassed = (yearsPassed * 365) + ((yearsPassed + 3) / 4) - ((yearsPassed + 99) / 100) + ((yearsPassed + 399) / 400);
        int daysPassed = monthsPassed;

        // Calculate the days passed for each month
        int[] monthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        for (int i = 1; i < date.month; i++)
        {
            daysPassed += monthDays[i - 1];
        }

        // Add the remaining days of the current month
        daysPassed += date.day - 1;

        return daysPassed;
    }

    public static void ToggleBool(ref bool value)
    {
        value = !value;
    }

    public static int WeightedRandom(params int[] values)
    {
        int sum = values.Sum();
        int value = Random.Range(0, sum);

        sum -= 1;
        for (int i = values.Length - 1; i > -1; i--)
        {
            if (values[i] < 1)
                continue;

            sum -= values[i];
            if (value > sum) return i;
        }

        return -1;
    }
}