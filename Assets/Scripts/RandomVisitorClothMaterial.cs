using UnityEngine;

public class RandomVisitorClothMaterial : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] renderers;
    [SerializeField] private Material[] materials;
    [SerializeField] private int slotIndexToChange;

    private void Start()
    {
        ApplyRandomMaterial();
    }

    private void ApplyRandomMaterial()
    {
        if (materials == null || materials.Length == 0)
        {
            Debug.LogWarning("No materials assigned!");
            return;
        }

        Material randomMaterial = materials[Random.Range(0, materials.Length)];

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (renderer == null) continue;

            Material[] mats = renderer.materials;

            if (slotIndexToChange < 0 || slotIndexToChange >= mats.Length)
            {
                Debug.LogWarning($"slotIndexToChange {slotIndexToChange} is out of bounds for renderer {renderer.name}.");
                continue;
            }

            mats[slotIndexToChange] = randomMaterial;
            renderer.materials = mats;
        }
    }
}