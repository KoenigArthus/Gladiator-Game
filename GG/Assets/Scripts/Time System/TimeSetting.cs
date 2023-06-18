using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "TimeSetting", menuName = "ScriptableObjects/TimeSetting")]
public class TimeSetting : ScriptableObject
{
    public Color lightColor;
    [ColorUsage (true,true)]
    public Color skyColor, equatorColor, groundColor;
    public float lightTemperature, lightIntensity;
    public Vector3 lightRotation;
    public Material skybox;
}
