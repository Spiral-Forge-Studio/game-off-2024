using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFlasher : MonoBehaviour
{
    public bool initializeMaterialsOnStart = false;
    public float flashInterval = 0.1f; // Time between flashes
    public int flashCount = 3; // Number of flashes
    public float emissionIntensity = 2.0f; // Intensity of the emission during flash
    private List<Material> allMaterials = new List<Material>(); // Store all materials
    private List<Color> originalColors = new List<Color>(); // Store original material colors
    private List<Color> originalEmissionColors = new List<Color>(); // Store original emission colors
    public Color flashColor;

    private void Start()
    {

    }

    /// <summary>
    /// Initializes and stores all materials from all child MeshRenderers and SkinnedMeshRenderers.
    /// </summary>
    public void InitializeMaterials()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        // Add materials from MeshRenderers
        foreach (var renderer in meshRenderers)
        {
            foreach (var material in renderer.materials)
            {
                allMaterials.Add(material);
                if (material.HasProperty("_Color"))
                {
                    originalColors.Add(material.color);
                }
                if (material.HasProperty("_EmissionColor"))
                {
                    originalEmissionColors.Add(material.GetColor("_EmissionColor"));
                }
                else
                {
                    originalEmissionColors.Add(Color.black);
                }
            }
        }

        // Add materials from SkinnedMeshRenderers
        foreach (var renderer in skinnedMeshRenderers)
        {
            foreach (var material in renderer.materials)
            {
                allMaterials.Add(material);
                if (material.HasProperty("_Color"))
                {
                    originalColors.Add(material.color);
                }
                if (material.HasProperty("_EmissionColor"))
                {
                    originalEmissionColors.Add(material.GetColor("_EmissionColor"));
                }
                else
                {
                    originalEmissionColors.Add(Color.black);
                }
            }
        }
    }

    /// <summary>
    /// Flashes all stored materials white with bright emission a specified number of times over a given interval.
    /// </summary>
    public void FlashAllMaterials()
    {
        if (allMaterials.Count == 0)
        {
            Debug.LogWarning("NoMaterials");

            return;
        }

        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            // Set materials to white with bright emission
            foreach (var material in allMaterials)
            {
                //Color flashColor = new Color(210f/255f, 210f / 255f, 210f / 255f, 1);

                if (material.HasProperty("_Color"))
                {
                    material.color = flashColor;
                }
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", flashColor * emissionIntensity);
                    material.EnableKeyword("_EMISSION");
                }
            }

            // Wait for half the interval (on time)
            yield return new WaitForSeconds(flashInterval / 2);

            // Restore original colors and emission
            for (int j = 0; j < allMaterials.Count; j++)
            {
                if (allMaterials[j].HasProperty("_Color"))
                {
                    allMaterials[j].color = originalColors[j];
                }
                if (allMaterials[j].HasProperty("_EmissionColor"))
                {
                    allMaterials[j].SetColor("_EmissionColor", originalEmissionColors[j]);
                    if (originalEmissionColors[j] == Color.black)
                    {
                        allMaterials[j].DisableKeyword("_EMISSION");
                    }
                }
            }

            // Wait for the remaining half of the interval (off time)
            yield return new WaitForSeconds(flashInterval / 2);
        }
    }
}
