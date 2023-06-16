using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TimeSetting", menuName = "ScriptableObjects/TimeSetting")]
public class TimeSetting : ScriptableObject
{
    public Color lightColor;
    public float lightTemperature, lightIntensity;
    public Vector3 lightRotation;
}
