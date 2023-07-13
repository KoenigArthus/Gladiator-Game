using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CustomUtility
{
    /// <summary>
    /// Converts a given string as "x,y,z" to a Vector3
    /// </summary>
    /// <param name="sVector"></param>
    /// <returns></returns>
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



    /// <summary>
    /// Replases all the spaces in the given sting with underscores
    /// </summary>
    /// <param name="text"></param>
    public static void ReplaceSpacesWithUnderscoresInplace(ref string text)
    {
        // Replace all spaces in the given string with underscores in place.
        text = text.Replace(' ', '_');
    }


    /// <summary>
    /// Converts the given date to a total number of passed days
    /// </summary>
    /// <param name="date"></param>
    /// <returns> the amount of days that have passed since the w0 d1 m1 y1</returns>
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

    /// <summary>
    /// Converts the total number of passed days into a Date.
    /// </summary>
    /// <param name="passedDays">The total number of days passed.</param>
    /// <returns>A Date struct representing the converted date.</returns>
    public static Date ToDate(int passedDays)
    {

        int yearsPassed = passedDays / 365;
        int remainingDays = passedDays % 365;

        int year = yearsPassed + 1;
        int month = 0;
        int day = 0;

        int[] monthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        for (int i = 0; i < monthDays.Length; i++)
        {
            if (remainingDays < monthDays[i])
            {
                month = i + 1;
                day = remainingDays + 1;
                break;
            }

            remainingDays -= monthDays[i];
        }

        Date date = new Date()
        {
            weekday = passedDays % 7,
            day = day,
            month = month,
            year = year
        };
        
        return date;
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


    public static void AddToDeckEntrie(string cardName)
    {
        List<string> temporaryList = UserFile.SaveGame.DeckCardEntries.ToList();

        if (CardLibrary.GetCardByName(cardName) != null)
            temporaryList.Add(CardLibrary.GetCardByName(cardName).Name);
        else
            Debug.LogWarning(cardName + " could not be found!");

        UserFile.SaveGame.DeckCardEntries = temporaryList.ToArray();

    }






}