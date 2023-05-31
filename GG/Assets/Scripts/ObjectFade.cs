using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class ObjectFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Material[] materials;
  // private Color[] initialDiffuseColors;
  // private int[] initialSurfaceValues;
    private Renderer objectRenderer;


    private void Start()
    {
        // Store the initial materials
        materials = gameObject.GetComponent<Renderer>().materials;

       
    }

    private void OnTriggerEnter(Collider collider)
    {

        for (int i = 0; i < materials.Length; i++)
        {
            Debug.Log(materials[i].name);
            materials[i].SetFloat("_Surface", 1);
        }

        //LeanTween.alpha(gameObject, 0f, 2f);
    }

    private void OnTriggerExit(Collider collider)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            Debug.Log(materials[i].name);
            materials[i].SetFloat("_Surface", 0);
        }
    }
}







































