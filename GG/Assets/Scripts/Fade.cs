using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject roof = null;
    public Material fadingMaterial = null;
    public float fadeDuration = 1f;
    private Material[] roofMaterials;
    private Color[] initialDiffuseColors;
    private int[] initialSurfaceValues;

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
            for (int i = 0; i < roofMaterials.Length; i++)
            {
                initialDiffuseColors[i] = roofMaterials[i].GetColor("_DiffuseColor");
                initialSurfaceValues[i] = roofMaterials[i].GetInt("_Surface");
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
        if (roofRenderer != null && fadingMaterial != null)
        {
            // Apply the fading material to the roof
            Material[] newMaterials = new Material[roofMaterials.Length];
            for (int i = 0; i < roofMaterials.Length; i++)
            {
                newMaterials[i] = fadingMaterial;
            }
            roofRenderer.materials = newMaterials;
        }
    }

    private bool IsCharacter(Collider collider)
    {
        // Implement your logic here to check if the collider is your player
        return true;
    }

    private System.Collections.IEnumerator FadeMaterials(float startAlpha, float targetAlpha, int surfaceValue)
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






































