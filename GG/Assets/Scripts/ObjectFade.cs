using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ObjectFade : MonoBehaviour
{
    public float fadeDuration = 1f;
    private List<Material> materials;
  // private Color[] initialDiffuseColors;
  // private int[] initialSurfaceValues;
    private MeshRenderer objectRenderer;


    private void Start()
    {
        // Store the initial materials
        objectRenderer = gameObject.GetComponent<MeshRenderer>();

        materials.Clear();

        for(int i = 0; i < objectRenderer.materials.Length; i++)
        {
            materials.Add(objectRenderer.materials[i]);
        }

       
    }

    private void OnTriggerEnter(Collider collider)
    {

        for (int i = 0; i < materials.Count; i++)
        {
            Debug.Log(materials[i].name);
        }

    }
/*
    private void ApplyFadingMaterial()
    {
        Renderer roofRenderer = roof.GetComponent<Renderer>();
        if (roofRenderer != null && fadingMaterials.Count > 0)
        {
            // Apply the fading materials to the roof
            Material[] newMaterials = new Material[roofMaterials.Length];
            for (int i = 0; i < roofMaterials.Length; i++)
            {
                if (roofMaterials[i].name == LUDUS_ROOFTILES_MATERIAL_NAME)
                    newMaterials[i] = fadingMaterialRooftiles;
                else if (roofMaterials[i].name == LUDUS_WALL_MATERIAL_NAME)
                    newMaterials[i] = fadingMaterialWall;
                else
                    newMaterials[i] = roofMaterials[i];
            }
            roofRenderer.materials = newMaterials;
        }
    }


    private bool IsCharacter(Collider collider)
    {
        // Implement your logic here to check if the collider is your player
        return true;
    }

    private IEnumerator FadeMaterials(float startAlpha, float targetAlpha, int surfaceValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;

            for (int i = 0; i < roofMaterials.Length; i++)
            {
                Material material = roofMaterials[i];

                // Set surface value to transparent
                material.SetInt("_Surface", surfaceValue);

                // Set blending mode to alpha for Ludus Wall material
                if (material.name == LUDUS_WALL_MATERIAL_NAME)
                {
                    material.EnableKeyword(ALPHA_BLEND_KEYWORD);
                }

                Color diffuseColor = initialDiffuseColors[i];
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
                diffuseColor.a = alpha;
                material.SetColor("_DiffuseColor", diffuseColor);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the materials reach the target alpha value exactly
        for (int i = 0; i < roofMaterials.Length; i++)
        {
            Material material = roofMaterials[i];
            Color diffuseColor = initialDiffuseColors[i];
            diffuseColor.a = targetAlpha;
            material.SetColor("_DiffuseColor", diffuseColor);
        }
    }

    private void ResetToOriginalMaterials()
    {
        Renderer roofRenderer = roof.GetComponent<Renderer>();
        if (roofRenderer != null)
        {
            // Restore the original materials of the roof
            roofRenderer.materials = roofMaterials;
        }
    }
*/
    private void OnTriggerExit(Collider collider)
    {
        
    }
}







































