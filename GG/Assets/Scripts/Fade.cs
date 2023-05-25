using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Fade : MonoBehaviour
{
    public GameObject roof = null;
    public Material fadingMaterialRooftiles = null;
    public Material fadingMaterialWall = null;
    public float fadeDuration = 1f;
    private Material[] roofMaterials;
    private Color[] initialDiffuseColors;
    private int[] initialSurfaceValues;
    private List<Material> replacedMaterials;
    private List<Material> fadingMaterials;

    private const string LUDUS_ROOFTILES_MATERIAL_NAME = "Ludus Rooftiles";
    private const string LUDUS_WALL_MATERIAL_NAME = "Ludus Wall";
    private const string ALPHA_BLEND_KEYWORD = "_ALPHA_BLEND";

    private void Start()
    {
        // Store the initial materials, diffuse colors, and surface values of the roof
        Renderer roofRenderer = roof.GetComponent<Renderer>();
        if (roofRenderer != null)
        {
            roofMaterials = roofRenderer.materials;
            initialDiffuseColors = new Color[roofMaterials.Length];
            initialSurfaceValues = new int[roofMaterials.Length];
            replacedMaterials = new List<Material>();
            fadingMaterials = new List<Material>();

            for (int i = 0; i < roofMaterials.Length; i++)
            {
                initialDiffuseColors[i] = roofMaterials[i].GetColor("_DiffuseColor");
                initialSurfaceValues[i] = roofMaterials[i].GetInt("_Surface");

                if (roofMaterials[i].name == LUDUS_ROOFTILES_MATERIAL_NAME)
                {
                    replacedMaterials.Add(roofMaterials[i]);
                    fadingMaterials.Add(fadingMaterialRooftiles);
                }
                else if (roofMaterials[i].name == LUDUS_WALL_MATERIAL_NAME)
                {
                    replacedMaterials.Add(roofMaterials[i]);
                    fadingMaterials.Add(fadingMaterialWall);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (IsCharacter(collider))
        {
            ApplyFadingMaterial();
            StartCoroutine(FadeMaterials(1f, 0f, 1));
        }
    }

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

    private void OnTriggerExit(Collider collider)
    {
        if (IsCharacter(collider))
        {
            StopAllCoroutines();
            StartCoroutine(FadeMaterials(0f, 1f, 0));
            ResetToOriginalMaterials();
        }
    }
}







































