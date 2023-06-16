using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] GameObject lightObject;
    private Light directionalLighting;
    public int currentTime { get => _currentTime; set { _currentTime = value; SetTimeTo(value); } }
    [SerializeField] private int _currentTime;
    public List<TimeSetting> timeSettings = new List<TimeSetting>();

    private void Awake()
    {
        directionalLighting = lightObject.GetComponent<Light>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentTime + 1 >= timeSettings.Count)
                currentTime = 0;
            else
                currentTime++;
        }
    }

    private void SetTimeTo(int time)
    {
        _currentTime = time;
        directionalLighting.color = timeSettings[time].lightColor;
        directionalLighting.colorTemperature = timeSettings[time].lightTemperature;
        directionalLighting.intensity = timeSettings[time].lightIntensity;
        lightObject.transform.rotation = Quaternion.Euler(timeSettings[time].lightRotation);
    }



}
