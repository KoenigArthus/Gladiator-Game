using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject lightObject;
    public Light directionalLighting;

    public Date currentDate;

    public int currentTime { get => _currentTime; set { _currentTime = value; SetTimeTo(value); } }
    [SerializeField] private int _currentTime;

    public List<TimeSetting> timeSettings = new List<TimeSetting>();

    private void Awake()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetNextTime();
        }
    }

    // Set the time to the specified value
    public void SetTimeTo(int time)
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


    // Start is called before the first frame update
    void Start()
    {
        // Set the initial current date to a specific date
        currentDate.day = 1;
        currentDate.month = 3;
        currentDate.year = 753;
    }



    public void SetCurrentDate(Date date)
    {
        if (IsValidDate(date))
        {
            // Assign the current date values to the provided Date object
            currentDate.day = date.day;
            currentDate.month = date.month;
            currentDate.year = date.year;
            Debug.Log("Set Date to: " + currentDate.day + "/" + currentDate.month + "/" + currentDate.year);
        }
        else
            Debug.LogError("Invalid date provided");



    }


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
                currentDate.day = 1;
                currentDate.month = 1;
                currentDate.year++;
            }
            else
            {
                currentDate.day = 1;
                currentDate.month++;
            }
        }
        else
        {
            currentDate.day++;
        }
    }

    public void SetPreviousDate()
    {
        // Check if it's the first day of the month
        bool firstDayOfMonth = (currentDate.day == 1);

        if (firstDayOfMonth)
        {
            // Check if it's the first month of the year
            if (currentDate.month == 1)
            {
                currentDate.day = 29;
                currentDate.month = 12;
                currentDate.year--;
            }
            else
            {
                currentDate.month--;
                switch (currentDate.month)
                {
                    case 1: // Ianuarius
                    case 4: // Aprilis
                    case 6: // Iunius
                    case 8: // Sextilis
                    case 9: // September
                    case 11: // November
                        currentDate.day = 29;
                        break;

                    case 2: // Februarius
                        currentDate.day = 28;
                        break;

                    case 3: // Martius
                    case 5: // Maius
                    case 7: // Quintilis
                    case 10: // October
                    case 12: // December
                        currentDate.day = 31;
                        break;
                }
            }
        }
        else
        {
            currentDate.day--;
        }
    }

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




}
