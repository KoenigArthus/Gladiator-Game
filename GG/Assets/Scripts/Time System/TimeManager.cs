using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    public GameObject lightObject;
    [HideInInspector] public Light directionalLighting;
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
            SetNextTime();
        }
    }

    public void SetTimeTo(int time)
    {
        _currentTime = time;
        directionalLighting.color = timeSettings[time].lightColor;
        directionalLighting.colorTemperature = timeSettings[time].lightTemperature;
        directionalLighting.intensity = timeSettings[time].lightIntensity;
        lightObject.transform.rotation = Quaternion.Euler(timeSettings[time].lightRotation);
        RenderSettings.skybox = timeSettings[time].skybox;
        RenderSettings.ambientSkyColor = timeSettings[time].skyColor;
        RenderSettings.ambientEquatorColor = timeSettings[time].equatorColor;
        RenderSettings.ambientGroundColor = timeSettings[time].groundColor;
        DynamicGI.UpdateEnvironment();
    }

    public void SetNextTime()
    {
        if (currentTime + 1 >= timeSettings.Count)
            currentTime = 0;
        else
            currentTime++;
    }

    public void SetPreviousTime()
    {
        if (currentTime - 1 < 0)
            currentTime = timeSettings.Count - 1;
        else
            currentTime = currentTime - 1;
    }


}
