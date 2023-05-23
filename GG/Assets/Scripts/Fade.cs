using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject roof = null;
    private Material[] roofMaterials;
    private Color[] initialDiffuseColors;

    private void Start()
    {
        // Store the initial materials and diffuse colors of the roof
        Renderer roofRenderer = roof.GetComponent<Renderer>();
        if (roofRenderer != null)
        {
            roofMaterials = roofRenderer.materials;
            initialDiffuseColors = new Color[roofMaterials.Length];
            for (int i = 0; i < roofMaterials.Length; i++)
            {
                initialDiffuseColors[i] = roofMaterials[i].GetColor("_DiffuseColor");
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (IsCharacter(collider))
        {
            SetMaterialTransparent();
            SetMaterialAlpha(0f);
        }
    }

    private bool IsCharacter(Collider collider)
    {
        // Implement your logic here to check if the collider is your player
        return true;
    }

    private void SetMaterialTransparent()
    {
        foreach (Material m in roofMaterials)
        {
            m.SetInt("_Surface", 1); // Set surface type to "Transparent"
            m.EnableKeyword("_BLENDMODE_ALPHA"); // Enable custom blend mode keyword
        }
    }

    private void SetMaterialOpaque()
    {
        for (int i = 0; i < roofMaterials.Length; i++)
        {
            Material m = roofMaterials[i];
            m.SetInt("_Surface", 0); // Set surface type to "Opaque"
            m.DisableKeyword("_BLENDMODE_ALPHA"); // Disable custom blend mode keyword
        }
    }

    private void SetMaterialAlpha(float alpha)
    {
        for (int i = 0; i < roofMaterials.Length; i++)
        {
            Material m = roofMaterials[i];
            Color diffuseColor = m.GetColor("_DiffuseColor");
            diffuseColor.a = alpha;
            m.SetColor("_DiffuseColor", diffuseColor);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (IsCharacter(collider))
        {
            SetMaterialAlpha(1f);
            SetMaterialOpaque();
        }
    }
}














