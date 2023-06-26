using System;
using System.Collections.Generic;
using UnityEngine;
using CustomAttributes;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    UnityEvent timeHasChanged;
    CharacterSpawnManager characterSpawner;
    public GameObject lightObject;
    public Light directionalLighting;
    [ReadOnly] public int passedDays;

    public Date currentDate { get => _currentDate; set { _currentDate = value; SetDate(value); timeHasChanged?.Invoke(); } }
    [SerializeField] private Date _currentDate;
    public int currentTime { get => _currentTime; set { _currentTime = value; SetTime(value); timeHasChanged?.Invoke(); } }
    [SerializeField] private int _currentTime;

    public List<TimeSetting> timeSettings = new List<TimeSetting>();

    private void Awake()
    {
        characterSpawner = GetComponent<CharacterSpawnManager>();
        if (timeHasChanged == null)
            timeHasChanged = new UnityEvent();

        timeHasChanged.AddListener(characterSpawner.ScheduleCharacters);
    }


    #region Time functions
    // Reset the Time to 0
    public void ResetTime()
    {
        currentTime = 0;
    }

    // Set the time to the specified value
    public void SetTime(int time)
    {
        _currentTime = time;

        // Update the directional lighting properties
        directionalLighting.color = timeSettings[time].lightColor;
        directionalLighting.colorTemperature = timeSettings[time].lightTemperature;
        directionalLighting.intensity = timeSettings[time].lightIntensity;

        // Rotate the light object
        lightObject.transform.rotation = Quaternion.Euler(timeSettings[time].lightRotation);

        // Update the skybox and ambient lighting settings
        RenderSettings.skybox = timeSettings[time].skybox;
        RenderSettings.ambientSkyColor = timeSettings[time].skyColor;
        RenderSettings.ambientEquatorColor = timeSettings[time].equatorColor;
        RenderSettings.ambientGroundColor = timeSettings[time].groundColor;

        // Update the dynamic global illumination
        DynamicGI.UpdateEnvironment();

        // Invoke timeHasChanged Event
        timeHasChanged?.Invoke();
    }
    // Set the time to the next time in the time settings list
    public void SetNextTime()
    {
        if (currentTime + 1 >= timeSettings.Count)
            currentTime = 0;
        else
            currentTime++;
    }

    // Set the time to the previous time in the time settings list
    public void SetPreviousTime()
    {
        if (currentTime - 1 < 0)
            currentTime = timeSettings.Count - 1;
        else
            currentTime = currentTime - 1;
    }
    #endregion Time functions

    #region Date funcitons

    // Resets Date to d1 m1 y1
    public void ResetDate()
    {
        _currentDate.weekday = 0;
        _currentDate.day = 1;
        _currentDate.month = 1;
        _currentDate.year = 1;
        passedDays = 0;
        // Set the current Date
        currentDate = _currentDate;
    }

    // Sets the Date to the given date
    public void SetDate(Date date)
    {
        if (IsValidDate(date))
        {
            _currentDate = date;

            // Calculate and assign the weekday value
            _currentDate.weekday = CalculateWeekday(date);
            // Calculate the total days passed and assign it to the passedDays variable
            passedDays = TotalDaysPassed(_currentDate);
            // Invoke timeHasChanged Event
            timeHasChanged?.Invoke();
            Debug.Log("Set Date to: " + _currentDate.day + "/" + currentDate.month + "/" + currentDate.year + " (Weekday: " + currentDate.weekday + ")");
        }
        else
        {
            Debug.LogError("Invalid date provided");
        }
    }

    // Advances a day in the currentdate
    public void SetNextDate()
    {
        // Check if it's the last day of the month
        bool lastDayOfMonth = false;

        switch (currentDate.month)
        {
            case 1: // Ianuarius
            case 4: // Aprilis
            case 6: // Iunius
            case 8: // Sextilis
            case 9: // September
            case 11: // November
                lastDayOfMonth = (currentDate.day == 29);
                break;

            case 2: // Februarius
                lastDayOfMonth = (currentDate.day == 28);
                break;

            case 3: // Martius
            case 5: // Maius
            case 7: // Quintilis
            case 10: // October
            case 12: // December
                lastDayOfMonth = (currentDate.day == 31);
                break;
        }

        if (lastDayOfMonth)
        {
            // Check if it's the last month of the year
            if (currentDate.month == 12)
            {
                _currentDate.day = 1;
                _currentDate.month = 1;
                _currentDate.year++;
            }
            else
            {
                _currentDate.day = 1;
                _currentDate.month++;
            }
        }
        else
        {
            _currentDate.day++;
        }


        // Calculate and assign the weekday value
        _currentDate.weekday = CalculateWeekday(currentDate);
        // Calculate the total days passed and assign it to the passedDays variable
        passedDays = TotalDaysPassed(currentDate);
        // Set the current Date
        currentDate = _currentDate;

    }

    // Goes a day back in the currentdate
    public void SetPreviousDate()
    {
        // Check if it's the first day of the month
        bool firstDayOfMonth = (currentDate.day == 1);

        if (firstDayOfMonth)
        {
            // Check if it's the first month of the year
            if (currentDate.month == 1)
            {
                _currentDate.day = 29;
                _currentDate.month = 12;
                _currentDate.year--;
            }
            else
            {
                _currentDate.month--;
                switch (currentDate.month)
                {
                    case 1: // Ianuarius
                    case 4: // Aprilis
                    case 6: // Iunius
                    case 8: // Sextilis
                    case 9: // September
                    case 11: // November
                        _currentDate.day = 29;
                        break;

                    case 2: // Februarius
                        _currentDate.day = 28;
                        break;

                    case 3: // Martius
                    case 5: // Maius
                    case 7: // Quintilis
                    case 10: // October
                    case 12: // December
                        _currentDate.day = 31;
                        break;
                }
            }
        }
        else
        {
            _currentDate.day--;
        }


        // Calculate and assign the weekday value
        _currentDate.weekday = CalculateWeekday(currentDate);
        // Calculate the total days passed and assign it to the passedDays variable
        passedDays = TotalDaysPassed(currentDate);
        // Set the current Date
        currentDate = _currentDate;
    }

    // Checks if the given date is possible
    bool IsValidDate(Date date)
    {
        // Check if the year is a positive value
        if (date.year <= 0)
        {
            return false;
        }

        // Check if the month is within the valid range
        if (date.month < 1 || date.month > 12)
        {
            return false;
        }

        // Check if the day is within the valid range for the given month
        switch (date.month)
        {
            case 1: // Ianuarius
            case 4: // Aprilis
            case 6: // Iunius
            case 8: // Sextilis
            case 9: // September
            case 11: // November
                if (date.day < 1 || date.day > 29)
                {
                    return false;
                }
                break;

            case 2: // Februarius
                if (date.day < 1 || date.day > 28)
                {
                    return false;
                }
                break;

            case 3: // Martius
            case 5: // Maius
            case 7: // Quintilis
            case 10: // October
            case 12: // December
                if (date.day < 1 || date.day > 31)
                {
                    return false;
                }
                break;

            default:
                return false;
        }

        return true;
    }

    // Calculates the weekday by the given date
    private int CalculateWeekday(Date date)
    {
        // Define the starting weekday (0 represents Monday)
        int startingWeekday = 0;

        // Calculate the number of days that have passed since the starting date
        int totalDays = TotalDaysPassed(date);

        // Calculate the weekday based on the starting weekday and the total number of days
        int weekday = (startingWeekday + totalDays) % 7;

        return weekday;
    }

    // gives back the number of days that have past since w0 d1 m1 y1
    private int TotalDaysPassed(Date date)
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

    #endregion Date functions

    #region Date & Time functions
    public void MoveInTime()
    {
        SetNextTime();

        if( currentTime == 0)
            SetNextDate();
    }
    public void MoveBackInTime()
    {
        SetPreviousTime();

        if(currentTime == 4)
            SetPreviousDate();
    }


    public void SetDateAndTime()
    {
        SetDate(currentDate);
        SetTime(currentTime);
    }
    public void SetTimeAndDate()
    {
        SetDateAndTime();
    }


    // Resets the Day and the Time 
    public void ResetDateAndTime()
    {
        ResetDate();
        ResetTime();
    }
    public void ResetTimeAndDate()
    {
        ResetDateAndTime();
    }

    #endregion Date & Time funcitons


}
